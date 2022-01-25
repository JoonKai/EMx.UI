// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.ClickableGridSplitter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.Controls
{
  public class ClickableGridSplitter : GridSplitter
  {
    private readonly Uri UriL = new Uri("pack://application:,,,/EMx.UI;component/Resources/box_left.png");
    private readonly Uri UriR = new Uri("pack://application:,,,/EMx.UI;component/Resources/box_right.png");
    private readonly Uri UriU = new Uri("pack://application:,,,/EMx.UI;component/Resources/box_up.png");
    private readonly Uri UriD = new Uri("pack://application:,,,/EMx.UI;component/Resources/box_down.png");

    public Dock DockProperty { get; set; }

    public bool IsCollapsed { get; set; }

    public GridLength ExpandedDefinition { get; set; }

    public ClickableGridSplitter()
    {
      this.Background = (Brush) Brushes.White;
      this.Loaded += new RoutedEventHandler(this.ClickableGridSplitter_Loaded);
      this.MouseDoubleClick += new MouseButtonEventHandler(this.ClickableGridSplitter_MouseDoubleClick);
      this.MinWidth = 10.0;
      this.MinHeight = 10.0;
    }

    public void SetBackground(Uri uri)
    {
      BitmapImage bitmapImage = new BitmapImage();
      bitmapImage.BeginInit();
      bitmapImage.UriSource = uri;
      bitmapImage.EndInit();
      ImageBrush imageBrush = new ImageBrush((ImageSource) bitmapImage);
      imageBrush.Stretch = Stretch.Uniform;
      this.Background = (Brush) imageBrush;
    }

    public void RefreshGrid()
    {
      if (this.IsCollapsed)
      {
        switch (this.DockProperty)
        {
          case Dock.Left:
            this.SetBackground(this.UriR);
            break;
          case Dock.Top:
            this.SetBackground(this.UriD);
            break;
          case Dock.Right:
            this.SetBackground(this.UriL);
            break;
          case Dock.Bottom:
            this.SetBackground(this.UriU);
            break;
        }
      }
      else
      {
        switch (this.DockProperty)
        {
          case Dock.Left:
            this.SetBackground(this.UriL);
            break;
          case Dock.Top:
            this.SetBackground(this.UriU);
            break;
          case Dock.Right:
            this.SetBackground(this.UriR);
            break;
          case Dock.Bottom:
            this.SetBackground(this.UriD);
            break;
        }
      }
    }

    private void ClickableGridSplitter_Loaded(object sender, RoutedEventArgs e) => this.RefreshGrid();

    private void ClickableGridSplitter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (!(this.Parent is Grid parent))
        return;
      switch (this.DockProperty)
      {
        case Dock.Left:
          int column1 = Grid.GetColumn((UIElement) this);
          if (column1 > 0)
            --column1;
          if (this.IsCollapsed)
          {
            parent.ColumnDefinitions[column1].Width = this.ExpandedDefinition;
            break;
          }
          this.ExpandedDefinition = parent.ColumnDefinitions[column1].Width;
          parent.ColumnDefinitions[column1].Width = new GridLength(0.0);
          break;
        case Dock.Top:
          int row1 = Grid.GetRow((UIElement) this);
          if (row1 > 0)
            --row1;
          if (this.IsCollapsed)
          {
            parent.RowDefinitions[row1].Height = this.ExpandedDefinition;
            break;
          }
          this.ExpandedDefinition = parent.RowDefinitions[row1].Height;
          parent.RowDefinitions[row1].Height = new GridLength(0.0);
          break;
        case Dock.Right:
          int column2 = Grid.GetColumn((UIElement) this);
          if (parent.ColumnDefinitions.Count > column2 + 1)
            ++column2;
          if (this.IsCollapsed)
          {
            parent.ColumnDefinitions[column2].Width = this.ExpandedDefinition;
            break;
          }
          this.ExpandedDefinition = parent.ColumnDefinitions[column2].Width;
          parent.ColumnDefinitions[column2].Width = new GridLength(0.0);
          break;
        case Dock.Bottom:
          int row2 = Grid.GetRow((UIElement) this);
          if (parent.RowDefinitions.Count > row2 + 1)
            ++row2;
          if (this.IsCollapsed)
          {
            parent.RowDefinitions[row2].Height = this.ExpandedDefinition;
            break;
          }
          this.ExpandedDefinition = parent.RowDefinitions[row2].Height;
          parent.RowDefinitions[row2].Height = new GridLength(0.0);
          break;
      }
      this.IsCollapsed = !this.IsCollapsed;
      this.RefreshGrid();
    }
  }
}
