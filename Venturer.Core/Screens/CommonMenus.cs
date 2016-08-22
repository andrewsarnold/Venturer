using System;
using System.Collections.Generic;

namespace Venturer.Core.Screens
{
    internal static class CommonMenus
    {
        internal static Menu SaveSlotPicker(string header, Action<int> onSelectAction, Action onEscapeAction)
        {
            return new Menu(header, new List<MenuOption>
            {
                new MenuOption("Slot 1", () => onSelectAction(1), false, true),
                new MenuOption("Slot 2", () => onSelectAction(2), false, true),
                new MenuOption("Slot 3", () => onSelectAction(3), false, true),
                new MenuOption("Back", () => { }, false, true)
            }, onEscapeAction);
        }
    }
}
