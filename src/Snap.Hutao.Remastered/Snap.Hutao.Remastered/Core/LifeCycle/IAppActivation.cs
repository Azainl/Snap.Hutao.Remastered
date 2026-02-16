// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Windows.AppNotifications;

namespace Snap.Hutao.Remastered.Core.LifeCycle;

public interface IAppActivation
{
    void RedirectedActivate(HutaoActivationArguments args);

    void NotificationInvoked(AppNotificationManager manager, AppNotificationActivatedEventArgs args);

    void ActivateAndInitialize(HutaoActivationArguments args);
}