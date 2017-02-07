using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Handlers.TaxCollector;

namespace Stump.Server.WorldServer.Game.Exchanges.TaxCollector
{
    public class TaxCollectorExchange : IExchange
    {
        private readonly CharacterCollector m_collector;

        public TaxCollectorExchange(TaxCollectorNpc taxCollector, Character character)
        {
            TaxCollector = taxCollector;
            Character = character;
            m_collector = new CharacterCollector(taxCollector, character, this);
        }

        public TaxCollectorNpc TaxCollector
        {
            get;
        }

        public Character Character
        {
            get;
        }

        private TimedTimerEntry Timer
        {
            get;
            set;
        }

        public ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.TAXCOLLECTOR;

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_EXCHANGE;

        #region IDialog Members

        public void Open()
        {
            Character.SetDialoger(m_collector);
            TaxCollector.OnDialogOpened(this);

            InventoryHandler.SendExchangeStartedMessage(Character.Client, ExchangeType);
            InventoryHandler.SendStorageInventoryContentMessage(Character.Client, TaxCollector);

            //Attention, la fenêtre d'échange se fermera automatiquement dans %1 minutes.
            Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 139, 2);
            Timer = Character.Area.CallDelayed((5 * 60 * 1000), Close);
        }

        public void Close()
        {
            Character.Area.UnregisterTimer(Timer);

            Character.CloseDialog(this);
            TaxCollector.OnDialogClosed(this);

            if (TaxCollector.IsDisposed)
                return;

            //<b>%3</b> a relevé la collecte sur le percepteur %1 en <b>%2</b> et recolté : %4
            TaxCollectorHandler.SendGetExchangeGuildTaxCollectorMessage(TaxCollector.Guild.Clients, TaxCollector);
            TaxCollectorHandler.SendTaxCollectorMovementMessage(TaxCollector.Guild.Clients, false, TaxCollector, Character.Name);

            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);

            TaxCollector.Guild.AddXP(TaxCollector.GatheredExperience);
            TaxCollector.Delete();
        }

        #endregion IDialog Members
    }
}