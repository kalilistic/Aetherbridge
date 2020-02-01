﻿using System.Linq;
using System.Text.RegularExpressions;

// ReSharper disable InvertIf

namespace ACT_FFXIV_Aetherbridge
{
	public abstract class LogLineParserBase
	{
		protected ILogLineParserContext Context;
		protected LogLineEvent LogLineEvent;

		protected LogLineParserBase(ILogLineParserContext context)
		{
			Context = context;
		}

		public void Parse(ACTLogLineEvent actLogLineEvent)
		{
			LogLineEvent = new LogLineEvent {ACTLogLineEvent = actLogLineEvent};
			LogLineEvent.LogMessage = LogLineEvent.ACTLogLineEvent.LogLine;
			ReplaceControlCharacters();
			ExtractTimestamp();
			ExtractLogCode();
			ExtractGameLogCode();
			RemoveRedundantNamePrefix();
			RemoveWorldName();
			ParseLootEvent();
			UpdateSpecialCharacters();
		}

		private void ReplaceControlCharacters()
		{
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace("\uE0BB ", string.Empty);
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace("\uE0BB", string.Empty);
		}

		private void ExtractTimestamp()
		{
			var match = Context.TimestampRegex.Match(LogLineEvent.LogMessage);
			LogLineEvent.Timestamp = match.Groups["Timestamp"].Value;
			LogLineEvent.LogMessage = match.Groups["Residual"].Value;
		}

		private void ExtractLogCode()
		{
			var match = Context.LogLineCodeRegex.Match(LogLineEvent.LogMessage);
			LogLineEvent.LogCode = match.Groups["LogCode"].Value;
			LogLineEvent.LogMessage = match.Groups["Residual"].Value;
		}

		private void ExtractGameLogCode()
		{
			if (!LogLineEvent.LogCode.Equals("00")) return;
			var match = Context.GameLogCodeRegex.Match(LogLineEvent.LogMessage);
			LogLineEvent.LogMessage = match.Groups["Residual"].Value;
			LogLineEvent.GameLogCode = match.Groups["GameLogCode"].Value;
		}

		private void RemoveRedundantNamePrefix()
		{
			if (LogLineEvent.GameLogCode == null || !LogLineEvent.GameLogCode.Equals("001d")) return;
			var name = LogLineEvent.LogMessage.Split(':')[0];
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Remove(0, name.Length + 1);
		}

		private void UpdateSpecialCharacters()
		{
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Trim();
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(" .", ".");
			LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace("\uE03C", Context.HQString);
		}

		private void RemoveWorldName()
		{
			if (!LogLineEvent.LogCode.Equals("00")) return;
			var matches = Context.ActorWithWorldNameRegex.Matches(LogLineEvent.LogMessage);

			foreach (Match match in matches)
			{
				var actorName = match.Groups["ActorName"].Value;
				var worldName = match.Groups["WorldName"].Value;
				LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace(actorName + worldName, actorName);
			}
		}

		protected void CleanUpLootEvent()
		{
			if (LogLineEvent == null) return;
			if (LogLineEvent?.XIVEvent?.Item == null)
				LogLineEvent.XIVEvent = null;
			else
				LogLineEvent.LogMessage = LogLineEvent.LogMessage.Replace("  ", " ");
		}

		protected void ParseRawItemName(Match match, DraftItem draftItem)
		{
			var rawItemName = match.Groups["RawItemName"].Value;
			rawItemName = rawItemName.Replace(" " + Context.HQChar, string.Empty);
			rawItemName = rawItemName.Replace(Context.HQChar, string.Empty);
			draftItem.RawItemName = rawItemName;
		}

		protected virtual void ParseItemNameAndQuantity(DraftItem draftItem)
		{
			var match = Context.ItemNameRegex.Match(draftItem.RawItemName);
			draftItem.ItemName = match.Groups["ItemName"].Value.Trim();
			var quantityStr = match.Groups["Quantity"].Value.Replace(Context.NumberDelimiterLocalized, string.Empty);
			try
			{
				draftItem.Quantity = int.Parse(quantityStr);
			}
			catch
			{
				draftItem.Quantity = 1;
			}
		}

		protected Item CreateItemFromDraft(DraftItem draftItem)
		{
			var item = FindItem(draftItem.ItemName, draftItem.Quantity);
			if (item == null) return null;
			item.Quantity = draftItem.Quantity;
			item.IsHQ = LogLineEvent.LogMessage.Contains(Context.HQChar);
			return item;
		}

		protected Item CreateItem(Match rawItemMatch)
		{
			var draftItem = new DraftItem();
			ParseRawItemName(rawItemMatch, draftItem);
			ParseItemNameAndQuantity(draftItem);
			return CreateItemFromDraft(draftItem);
		}

		protected virtual Player CreateActor(Match actorMatch)
		{
			var actorName = actorMatch.Groups["ActorNameWithWorldName"].Value;
			var currentPlayer = Context.Aetherbridge.PlayerService.GetCurrentPlayer();

			if (actorName.Equals(string.Empty))
			{
				LogLineEvent.LogMessage = currentPlayer.Name + LogLineEvent.LogMessage;
				return currentPlayer;
			}

			if (actorName.ToUpper().Equals(Context.YouLocalized))
			{
				LogLineEvent.LogMessage = LogLineEvent.LogMessage.Remove(0, Context.YouLocalized.Length);
				LogLineEvent.LogMessage = currentPlayer.Name + LogLineEvent.LogMessage;
				return currentPlayer;
			}

			return actorName.Equals(currentPlayer.Name)
				? currentPlayer
				: new Player {Name = actorName, IsReporter = false};
		}

		protected bool IsFalsePositive()
		{
			return Context.LootFalsePositives.Any(falsePositive => LogLineEvent.LogMessage.Contains(falsePositive));
		}

		protected bool IsAddLoot()
		{
			var match = Context.ItemAddedRegex.Match(LogLineEvent.LogMessage);
			if (match.Success)
				LogLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.AddLoot)
				{
					Item = CreateItem(match)
				};

			return match.Success;
		}

		protected bool IsLostLoot()
		{
			var match = Context.UnableToObtainRegex.Match(LogLineEvent.LogMessage);
			if (match.Success)
				LogLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.LostLoot)
				{
					Item = CreateItem(match)
				};

			return match.Success;
		}

		protected bool IsObtainMostRareLoot()
		{
			var match = Context.ObtainWithMostRareRegex.Match(LogLineEvent.LogMessage);
			if (match.Success)
			{
				LogLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.ObtainLoot)
				{
					Item = CreateItem(match),
					Actor = CreateActor(match)
				};
				NormalizeObtainWithMostRare();
			}

			return match.Success;
		}

		protected virtual bool IsObtainLoot()
		{
			var match = Context.ObtainRegex.Match(LogLineEvent.LogMessage);
			if (match.Success)
			{
				LogLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.ObtainLoot)
				{
					Item = CreateItem(match),
					Actor = CreateActor(match)
				};
				NormalizeObtain();
			}

			return match.Success;
		}

		protected bool IsGreedLoot()
		{
			var match = Context.GreedRegex.Match(LogLineEvent.LogMessage);
			if (match.Success)
			{
				LogLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.GreedLoot)
				{
					Item = CreateItem(match),
					Actor = CreateActor(match),
					Roll = int.Parse(match.Groups["Roll"].Value)
				};
				NormalizeRoll();
			}

			return match.Success;
		}

		protected bool IsNeedLost()
		{
			var match = Context.NeedRegex.Match(LogLineEvent.LogMessage);
			if (match.Success)
			{
				LogLineEvent.XIVEvent = new XIVEvent(XIVEventTypeEnum.Loot, XIVEventSubTypeEnum.NeedLoot)
				{
					Item = CreateItem(match),
					Actor = CreateActor(match),
					Roll = int.Parse(match.Groups["Roll"].Value)
				};
				NormalizeRoll();
			}

			return match.Success;
		}

		private void ParseLootEvent()
		{
			if (IsFalsePositive()) return;

			if (IsAddLoot() ||
			    IsLostLoot() ||
			    IsObtainMostRareLoot() ||
			    IsObtainLoot() ||
			    IsGreedLoot() ||
			    IsNeedLost())
				CleanUpLootEvent();
		}

		public abstract void NormalizeObtainWithMostRare();
		public abstract void NormalizeObtain();
		public abstract void NormalizeRoll();
		public abstract Item FindItem(string itemName, int quantity);
	}
}