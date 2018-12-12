using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
namespace coffee_pc.Utils
{
    public class Toast
    {
        static public Notifier ProvideToast() {
            Notifier notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.BottomCenter,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(2),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(2));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
            return notifier;
        }


    }
}
