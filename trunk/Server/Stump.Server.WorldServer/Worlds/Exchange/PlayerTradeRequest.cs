using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Dialog;

namespace Stump.Server.WorldServer.Worlds.Exchange
{
    public class PlayerTradeRequest : IRequestBox
    {
        public PlayerTradeRequest(Character source, Character target)
        {
            Source = source;
            Target = target;
        }

        #region IRequestBox Members

        public Character Source
        {
            get;
            private set;
        }

        public Character Target
        {
            get;
            private set;
        }

        public void Open()
        {
            InventoryHandler.SendExchangeRequestedTradeMessage(Source.Client, ExchangeTypeEnum.PLAYER_TRADE,
                                                               Source, Target);
            InventoryHandler.SendExchangeRequestedTradeMessage(Target.Client, ExchangeTypeEnum.PLAYER_TRADE,
                                                               Source, Target);
        }

        public void Accept()
        {
            var trade = TradeManager.Instance.Create();

            var firstTrader = new PlayerTrader(Source, trade);
            Source.SetDialoger(firstTrader);

            var secondTrader = new PlayerTrader(Target, trade);
            Target.SetDialoger(secondTrader);

            trade.FirstTrader = firstTrader;
            trade.SecondTrader = secondTrader;

            trade.Open();

            Close();
        }

        public void Deny()
        {
            InventoryHandler.SendExchangeLeaveMessage(Source.Client, false);
            InventoryHandler.SendExchangeLeaveMessage(Target.Client, false);

            Close();
        }

        public void Cancel()
        {
            Deny();
        }

        public void Close()
        {
            Source.ResetRequestBox();
            Target.ResetRequestBox();
        }

        #endregion
    }
}