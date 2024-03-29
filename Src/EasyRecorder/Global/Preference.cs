﻿using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ESR.Global
{
    public class Preference : SingletonBase<Preference>
    {
        public bool IsTopMost { get; set; } = true;

        public bool IsShowToastMessage { get; set; } = true;

        public DoubleClickActionType DoubleClickAction { get; set; } = DoubleClickActionType.Memo;

        public string CommandKeyTextColor = "#3393DF";

        public RepeatType RepeatType { get; set; } = RepeatType.Infinite;

        public ButtonType DragClickButtonType = ButtonType.Right;

        public int DragClickNumberOfClicks = 10;

        public int DragClickClickTimeIntervalMsec = 50;

        [JsonIgnore]
        public Window MainWindow { get; set; } = null;

        [JsonIgnore]
        public Window LogWindow { get; set; } = null;

        [JsonIgnore]
        public MenuItem MenuAlwaysTopItem { get; set; } = null;

        [JsonIgnore]
        public MenuItem MenuInfiniteRepeatItem { get; set; } = null;

        [JsonIgnore]
        public MenuItem MenuRepeatCountItem { get; set; } = null;

        [JsonIgnore]
        public NumericUpDown RepeatCountControl { get; set; } = null;

        public bool Load()
        {
            return Adjust();
        }

        public bool Adjust()
        {
            this.MenuAlwaysTopItem.IsChecked = this.IsTopMost;
            this.MenuInfiniteRepeatItem.IsChecked = this.RepeatType == RepeatType.Infinite;
            this.MenuRepeatCountItem.IsChecked = this.RepeatType == RepeatType.Count;
            this.MainWindow.Topmost = this.IsTopMost;
            this.LogWindow.Topmost = this.IsTopMost;
            this.RepeatCountControl.IsEnabled = this.RepeatType == RepeatType.Count;

            AUtil.MoveToLeftBottom(this.LogWindow);
            return true;
        }

    }
}
