// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

// This code is based on http://nroute.codeplex.com/

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Windows.Input;
using LogoFX.Client.Core;
using LogoFX.Client.Mvvm.Core;

namespace LogoFX.Client.Mvvm.Commanding
{
    public static class ActionCommandExtensions
    {
        public static T RequeryOnPropertyChanged<T>(this T command, INotifyPropertyChanged notifiable)
            where 
                T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotDefault(notifiable, "notifiable");

            Observable
                .FromEventPattern<PropertyChangedEventHandler,PropertyChangedEventArgs>((a)=>notifiable.PropertyChanged+=a, (a)=>notifiable.PropertyChanged-=a)
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs));
            return command;
        }

        //todo:extend to better granularity - support for all properties on object path 
        public static T RequeryOnPropertyChanged<T>(this T command,object source, string path)
            where
                T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotDefault(source, "source");
            source.NotifyOn(path, (a,b) => command.ReceiveWeakEvent(new EventArgs()));
            return command;
        }

        public static T RequeryOnPropertyChanged<T>(this T command,
            INotifyPropertyChanged notifiable, Expression<Func<Object>> propertySelector)
            where 
                T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(notifiable, "notifiable");
            Guard.ArgumentNotNull(propertySelector, "propertySelector");

            Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>((a) => notifiable.PropertyChanged += a, (a) => notifiable.PropertyChanged -= a)
                .Where(a=>a.EventArgs.PropertyName == propertySelector.GetPropertyName())
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs));

            return command;
        }

        public static T RequeryOnCommandCanExecuteChanged<T>(this T command, ICommand relatedCommand)
            where 
                T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(relatedCommand, "relatedCommand");

            Observable
                .FromEventPattern<EventHandler, EventArgs>((a) => relatedCommand.CanExecuteChanged += a, (a) => relatedCommand.CanExecuteChanged -= a)
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs));

            return command;
        }

        public static T RequeryOnCommandExecuted<T>(this T command, IActionCommand relatedCommand)
            where 
                T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(relatedCommand, "relatedCommand");

            Observable
                .FromEventPattern<EventHandler<CommandEventArgs>,CommandEventArgs>((a) => relatedCommand.CommandExecuted += a, (a) => relatedCommand.CommandExecuted -= a)
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs));

            return command;
        }

        public static T RequeryWhenExecuted<T>(this T command)
            where 
                T : IActionCommand
        {
            Guard.ArgumentNotDefault(command, "command");

            Observable
                .FromEventPattern<EventHandler<CommandEventArgs>, CommandEventArgs>((a) => command.CommandExecuted += a, (a) => command.CommandExecuted -= a)
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs));

            return command;
        }

        public static T RequeryOnCollectionChanged<T>(this T command, INotifyCollectionChanged collection)
            where 
                T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(collection, "collection");

            Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>((a) => collection.CollectionChanged += a, (a) => collection.CollectionChanged -= a)
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs));

            return command;
        }


        public static T WithImage<T>(this T command, Uri image)
            where
                T : IExtendedCommand
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(image, "image");
            command.ImageUri = image; 

            return command;
        }

        public static T WithName<T>(this T command, string name)
            where
                T : IExtendedCommand
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(name, "name");
            command.Name = name;

            return command;
        }

        public static T WithDescription<T>(this T command, string description)
            where
                T : IExtendedCommand
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(description, "description");
            command.Description = description;

            return command;
        }

        public static T AsAdvanced<T>(this T command)
            where
                T : IExtendedCommand
        {
            Guard.ArgumentNotDefault(command, "command");

            command.IsAdvanced = true;

            return command;
        }
    }
}
