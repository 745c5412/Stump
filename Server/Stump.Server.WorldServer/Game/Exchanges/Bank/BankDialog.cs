using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Bank
{
    public class BankDialog : IExchange
    {
        public BankDialog(Character character)
        {
            Customer = new BankCustomer(character, this);
        }

        public Character Character
        {
            get { return Customer.Character; }
        }

        public BankCustomer Customer
        {
            get;
        }

        public ExchangeTypeEnum ExchangeType
        {
            get { return ExchangeTypeEnum.STORAGE; }
        }

        public DialogTypeEnum DialogType
        {
            get { return DialogTypeEnum.DIALOG_EXCHANGE; }
        }

        public void Open()
        {
            InventoryHandler.SendExchangeStartedMessage(Character.Client, ExchangeType);
            InventoryHandler.SendStorageInventoryContentMessage(Character.Client, Customer.Character.Bank);
            Character.SetDialoger(Customer);
        }

        public void Close()
        {
            Character.ResetDialog();
        }
    }
}