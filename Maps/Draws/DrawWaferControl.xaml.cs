// Decompiled with JetBrains decompiler
// Type: EMx.UI.Maps.Draws.DrawWaferControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EMx.UI.Maps.Draws
{
    public partial class DrawWaferControl : UserControl, IComponentConnector
    {
        private bool _contentLoaded;

        public virtual double WaferSize { get; set; }

        public virtual double EdgeExclusion { get; set; }

        public virtual double CellWidth { get; set; }

        public virtual double CellHeight { get; set; }

        public virtual double HorizontalLineThickness { get; set; }

        public virtual double VerticalLineThickness { get; set; }

        public DrawWaferControl()
        {
            this.InitializeComponent();
            this.WaferSize = 100.0;
            this.EdgeExclusion = 4.0;
            this.CellWidth = 2.0;
            this.CellHeight = 2.0;
            this.HorizontalLineThickness = 1.0;
            this.VerticalLineThickness = 1.0;
        }
    }

}
