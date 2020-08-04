using MvvmHelpers;
using Xamarin.Forms;

namespace CG4U.Security.ClientApp.Models
{
    public class Alert : ObservableObject
    {
        private int _id;
        private string _message;
        private AlertType _type;
        private AlertProcessingMethod _processingMethod;
        private int _idImageProcesses;

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        public AlertType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }
        public AlertProcessingMethod ProcessingMethod
        {
            get { return _processingMethod; }
            set { SetProperty(ref _processingMethod, value); }
        }
        public int IdImageProcesses
        {
            get { return _idImageProcesses; }
            set { SetProperty(ref _idImageProcesses, value); }
        }
        public string TypeTitle
        {
            get
            {
                switch (_type)
                {
                    case AlertType.Warning: return "Notificação";
                    case AlertType.Critical: return "Aviso";
                    case AlertType.Panic: return "Imediato";
                    default: return string.Empty;
                }
            }
        }
        public string TypeImageFile
        {
            get
            {
                switch (_type)
                {
                    case AlertType.Warning: return "icon_warning.png";
                    case AlertType.Critical: return "icon_critical.png";
                    case AlertType.Panic: return "icon_panic.png";
                    default: return string.Empty;
                }
            }
        }
        public string ProcessingMethodImageFile
        {
            get
            {
                switch (_processingMethod)
                {
                    case AlertProcessingMethod.SceneChange: return "icon_scenechange.png";
                    case AlertProcessingMethod.UnkownCar: return "icon_unkowncar.png";
                    case AlertProcessingMethod.UnkownPeople: return "icon_unkownpeople.png";
                    default: return string.Empty;
                }
            }
        }
    }
}
