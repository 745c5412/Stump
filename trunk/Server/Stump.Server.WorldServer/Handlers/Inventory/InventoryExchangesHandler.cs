using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Exchange;
using Item = Stump.Server.WorldServer.Worlds.Items.Item;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler
    {
        [WorldHandler(ExchangePlayerRequestMessage.Id)]
        public static void HandleExchangePlayerRequestMessage(WorldClient client, ExchangePlayerRequestMessage message)
        {
            switch ((ExchangeTypeEnum) message.exchangeType)
            {
                case ExchangeTypeEnum.PLAYER_TRADE:
                    var target = World.Instance.GetCharacter(message.target);

                    if (target == null)
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.BID_SEARCH_ERROR);
                        return;
                    }

                    if (target.Map.Id != client.ActiveCharacter.Map.Id)
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_TOOL_TOO_FAR);
                        return;
                    }

                    if (target.IsInRequest() || target.IsTrading())
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_OCCUPIED);
                        return;
                    }

                    var request = new PlayerTradeRequest(client.ActiveCharacter, target);
                    client.ActiveCharacter.OpenRequestBox(request);
                    target.OpenRequestBox(request);

                    request.Open();

                    break;
                default:
                    SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_IMPOSSIBLE);
                    break;
            }
        }

        [WorldHandler(ExchangeAcceptMessage.Id)]
        public static void HandleExchangeAcceptMessage(WorldClient client, ExchangeAcceptMessage message)
        {
            if (client.ActiveCharacter.IsInRequest() &&
                client.ActiveCharacter.RequestBox is PlayerTradeRequest)
            {
                client.ActiveCharacter.AcceptRequest();
            }
        }

        [WorldHandler(ExchangeObjectMoveKamaMessage.Id)]
        public static void HandleExchangeObjectMoveKamaMessage(WorldClient client, ExchangeObjectMoveKamaMessage message)
        {
            if (!client.ActiveCharacter.IsTrading())
                return;
            
            client.ActiveCharacter.Trader.SetKamas((uint) message.quantity);
        }

        [WorldHandler(ExchangeObjectMoveMessage.Id)]
        public static void HandleExchangeObjectMoveMessage(WorldClient client, ExchangeObjectMoveMessage message)
        {
            if (!client.ActiveCharacter.IsTrading())
                return;

            client.ActiveCharacter.Trader.MoveItem(message.objectUID, message.quantity);
        }

        [WorldHandler(ExchangeReadyMessage.Id)]
        public static void HandleExchangeReadyMessage(WorldClient client, ExchangeReadyMessage message)
        {
           client.ActiveCharacter.Trader.ToggleReady(message.ready);
        }

        /*[WorldHandler(ExchangeBuyMessage.Id)]
        public static void HandleExchangeBuyMessage(WorldClient client, ExchangeBuyMessage message)
        {
            ((NpcShopDialog) client.ActiveCharacter.Dialog).BuyItem((int) message.objectToBuyId, message.quantity);
        }

        [WorldHandler(ExchangeSellMessage.Id)]
        public static void HandleExchangeSellMessage(WorldClient client, ExchangeSellMessage message)
        {
            ((NpcShopDialog) client.ActiveCharacter.Dialog).SellItem((int) message.objectToSellId, message.quantity);
        }*/

        public static void SendExchangeRequestedTradeMessage(WorldClient client, ExchangeTypeEnum type, Character source,
                                                             Character target)
        {
            client.Send(new ExchangeRequestedTradeMessage(
                            (byte) type,
                            source.Id,
                            target.Id));
        }

        public static void SendExchangeStartedWithPodsMessage(WorldClient client, PlayerTrade playerTrade)
        {
            client.Send(new ExchangeStartedWithPodsMessage(
                            (byte) ExchangeTypeEnum.PLAYER_TRADE,
                            playerTrade.FirstTrader.Character.Id,
                            (int) playerTrade.FirstTrader.Character.Inventory.Weight,
                            (int) playerTrade.FirstTrader.Character.Inventory.WeightTotal,
                            playerTrade.SecondTrader.Character.Id,
                            (int) playerTrade.SecondTrader.Character.Inventory.Weight,
                            (int) playerTrade.SecondTrader.Character.Inventory.WeightTotal
                            ));
        }

        public static void SendExchangeStartOkNpcShopMessage(WorldClient client, int npcId, int tokenId,
                                                             List<ObjectItemToSellInNpcShop> items)
        {
            client.Send(new ExchangeStartOkNpcShopMessage(npcId, tokenId, items));
        }

        public static void SendExchangeLeaveMessage(WorldClient client, bool success)
        {
            client.Send(new ExchangeLeaveMessage(success));
        }

        public static void SendExchangeObjectAddedMessage(WorldClient client, bool remote, Item item)
        {
            client.Send(new ExchangeObjectAddedMessage(remote, item.GetObjectItem()));
        }

        public static void SendExchangeObjectModifiedMessage(WorldClient client, bool remote, Item item)
        {
            client.Send(new ExchangeObjectModifiedMessage(remote, item.GetObjectItem()));
        }

        public static void SendExchangeObjectRemovedMessage(WorldClient client, bool remote, int guid)
        {
            client.Send(new ExchangeObjectRemovedMessage(remote, guid));
        }

        public static void SendExchangeIsReadyMessage(WorldClient client, ITrader trader, bool ready)
        {
            client.Send(new ExchangeIsReadyMessage(trader.Actor.Id, ready));
        }

        public static void SendExchangeErrorMessage(WorldClient client, ExchangeErrorEnum errorEnum)
        {
            client.Send(new ExchangeErrorMessage((byte) errorEnum));
        }
    }
}