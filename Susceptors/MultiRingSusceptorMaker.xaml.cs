// Decompiled with JetBrains decompiler
// Type: EMx.UI.Susceptors.MultiRingSusceptorMaker
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Helpers;
using Microsoft.Win32;
using PH.DataTree;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit.Primitives;

namespace EMx.UI.Susceptors
{
  public partial class MultiRingSusceptorMaker : 
    UserControl,
    INotifyPropertyChanged,
    IComponentConnector
  {
    private double waferSize;
    private double susceptorSize;
    private double initialAngle;
    private CurrentRingInfo currentRing;
    internal ListBox lbxItems;
    internal Button btnAddRing;
    internal Button btnDeleteRing;
    internal Button btnClearRing;
    internal ComboBox cmbCurrentFlat;
    internal ComboBox cmbCurrentRotationDirection;
    internal Button btnReorder;
    internal Button btnSave;
    internal Button btnLoad;
    private bool _contentLoaded;

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public event EventHandler SusceptorChanged;

    private void OnSusceptorChanged()
    {
      EventHandler susceptorChanged = this.SusceptorChanged;
      if (susceptorChanged != null)
        susceptorChanged((object) this, new EventArgs());
      this.CurrentSusceptor.SelectRingGroup(this.lbxItems.SelectedIndex);
    }

    public SusceptorBase CurrentSusceptor { get; set; }

    public double WaferSize
    {
      get => this.waferSize;
      set
      {
        this.waferSize = value;
        this.OnPropertyChanged(nameof (WaferSize));
      }
    }

    public double SusceptorSize
    {
      get => this.susceptorSize;
      set
      {
        this.susceptorSize = value;
        this.OnPropertyChanged(nameof (SusceptorSize));
      }
    }

    public double InitialAngle
    {
      get => this.initialAngle;
      set
      {
        this.initialAngle = value;
        this.OnPropertyChanged(nameof (InitialAngle));
      }
    }

    public CurrentRingInfo CurrentRing
    {
      get => this.currentRing;
      set
      {
        this.currentRing = value;
        this.OnPropertyChanged(nameof (CurrentRing));
      }
    }

    public MultiRingSusceptorMaker()
    {
      this.CurrentSusceptor = new SusceptorBase();
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.WaferSize = 2.0;
      this.SusceptorSize = 11.0;
      this.CurrentRing = new CurrentRingInfo();
    }

    private void btnAddRing_Click(object sender, RoutedEventArgs e)
    {
      if (this.CurrentSusceptor.WaferList.Count == 0)
        this.CurrentSusceptor.AddRoot(this.SusceptorSize / 2.0);
      this.CurrentSusceptor.AddSubRingGroup(this.WaferSize, this.InitialAngle, true);
      this.OnSusceptorChanged();
      this.RefreshListBox();
    }

    private void RefreshListBox()
    {
      this.lbxItems.Items.Clear();
      List<SingleCirclePositioner> list = this.CurrentSusceptor.Tree.BreadthFirstEnumerator.Where<SingleCirclePositioner>((Func<SingleCirclePositioner, bool>) (x => x.PositionerType == SingleCirclePositionerType.Group)).ToList<SingleCirclePositioner>();
      for (int index = 0; index < list.Count; ++index)
        this.lbxItems.Items.Add((object) ("Group " + (object) (index + 1)));
    }

    private void btnDeleteRing_Click(object sender, RoutedEventArgs e)
    {
      if (this.lbxItems.SelectedIndex < 0)
        return;
      int selectedIndex = this.lbxItems.SelectedIndex;
      List<DTreeNode<SingleCirclePositioner>> list = this.CurrentSusceptor.Tree.BreadthFirstNodeEnumerator.Where<DTreeNode<SingleCirclePositioner>>((Func<DTreeNode<SingleCirclePositioner>, bool>) (x => x.Value.PositionerType == SingleCirclePositionerType.Group)).ToList<DTreeNode<SingleCirclePositioner>>();
      if (list.Count > selectedIndex)
      {
        DTreeNode<SingleCirclePositioner> node = list[selectedIndex];
        foreach (DTreeNode<SingleCirclePositioner> dtreeNode in node.BreadthFirstNodeEnumerator.ToList<DTreeNode<SingleCirclePositioner>>())
        {
          DTreeNode<SingleCirclePositioner> ring = dtreeNode;
          this.CurrentSusceptor.WaferList.Remove(ring.Value);
          if (node.Nodes.Any<DTreeNode<SingleCirclePositioner>>((Func<DTreeNode<SingleCirclePositioner>, bool>) (X => X.Value == ring.Value)))
            node.Nodes.Remove(ring);
        }
        if (node.Parent != null)
          node.Parent.Nodes.Remove(node);
      }
      this.OnSusceptorChanged();
      this.RefreshListBox();
      if (this.lbxItems.Items.Count > 0)
        this.lbxItems.SelectedIndex = this.lbxItems.Items.Count - 1;
    }

    private void btnClearRing_Click(object sender, RoutedEventArgs e)
    {
      this.CurrentSusceptor = new SusceptorBase();
      this.CurrentSusceptor.AddRoot(this.SusceptorSize / 2.0);
      this.OnSusceptorChanged();
      this.RefreshListBox();
    }

    private void DoubleUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (this.CurrentSusceptor.Tree.Root.RootNode.Value == null)
        return;
      SingleCirclePositioner circlePositioner = this.CurrentSusceptor.Tree.Root.RootNode.Value;
      circlePositioner.Radius = this.SusceptorSize;
      circlePositioner.Position = new Point(this.SusceptorSize, this.SusceptorSize);
      circlePositioner.StackedPosition = circlePositioner.Position;
      this.OnSusceptorChanged();
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "susceptor files|*.susc|all files|*.*";
      bool? nullable = saveFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      Helper.IO.SaveObjectToXml((object) this.CurrentSusceptor, saveFileDialog.FileName);
    }

    private void btnLoad_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "susceptor files|*.susc|all files|*.*";
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      SusceptorBase susceptorBase = Helper.IO.LoadObjectFromXml<SusceptorBase>(openFileDialog.FileName);
      if (susceptorBase != null)
      {
        susceptorBase.ConnectEvent();
        this.CurrentSusceptor = susceptorBase;
        this.OnSusceptorChanged();
        this.RefreshListBox();
      }
    }

    private void DoubleUpDown_ValueChanged_1(
      object sender,
      RoutedPropertyChangedEventArgs<object> e)
    {
      if (this.lbxItems.SelectedIndex < 0)
        return;
      this.CurrentRing.IsCCW = this.cmbCurrentRotationDirection.SelectedIndex == 0;
      this.CurrentRing.IsIn = this.cmbCurrentFlat.SelectedIndex == 0;
      this.CurrentSusceptor.ModifyRingGroup(this.lbxItems.SelectedIndex, this.CurrentRing);
      this.OnSusceptorChanged();
    }

    private void cmbFlat_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.DoubleUpDown_ValueChanged_1((object) null, (RoutedPropertyChangedEventArgs<object>) null);

    private void lbxItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.lbxItems.SelectedIndex < 0)
        return;
      this.CurrentRing = this.CurrentSusceptor.GetRingGroupInfo(this.lbxItems.SelectedIndex);
      this.CurrentSusceptor.SelectRingGroup(this.lbxItems.SelectedIndex);
    }

    private void btnReorder_Click(object sender, RoutedEventArgs e)
    {
      this.CurrentSusceptor.SortIndex();
      this.OnSusceptorChanged();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/susceptors/multiringsusceptormaker.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.lbxItems = (ListBox) target;
          this.lbxItems.SelectionChanged += new SelectionChangedEventHandler(this.lbxItems_SelectionChanged);
          break;
        case 2:
          this.btnAddRing = (Button) target;
          this.btnAddRing.Click += new RoutedEventHandler(this.btnAddRing_Click);
          break;
        case 3:
          this.btnDeleteRing = (Button) target;
          this.btnDeleteRing.Click += new RoutedEventHandler(this.btnDeleteRing_Click);
          break;
        case 4:
          this.btnClearRing = (Button) target;
          this.btnClearRing.Click += new RoutedEventHandler(this.btnClearRing_Click);
          break;
        case 5:
          ((UpDownBase<double?>) target).ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.DoubleUpDown_ValueChanged);
          break;
        case 6:
          ((UpDownBase<double?>) target).ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.DoubleUpDown_ValueChanged);
          break;
        case 7:
          ((UpDownBase<double?>) target).ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.DoubleUpDown_ValueChanged_1);
          break;
        case 8:
          ((UpDownBase<double?>) target).ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.DoubleUpDown_ValueChanged_1);
          break;
        case 9:
          ((UpDownBase<int?>) target).ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.DoubleUpDown_ValueChanged_1);
          break;
        case 10:
          this.cmbCurrentFlat = (ComboBox) target;
          this.cmbCurrentFlat.SelectionChanged += new SelectionChangedEventHandler(this.cmbFlat_Copy_SelectionChanged);
          break;
        case 11:
          ((UpDownBase<double?>) target).ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.DoubleUpDown_ValueChanged_1);
          break;
        case 12:
          this.cmbCurrentRotationDirection = (ComboBox) target;
          this.cmbCurrentRotationDirection.SelectionChanged += new SelectionChangedEventHandler(this.cmbFlat_Copy_SelectionChanged);
          break;
        case 13:
          this.btnReorder = (Button) target;
          this.btnReorder.Click += new RoutedEventHandler(this.btnReorder_Click);
          break;
        case 14:
          this.btnSave = (Button) target;
          this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
          break;
        case 15:
          this.btnLoad = (Button) target;
          this.btnLoad.Click += new RoutedEventHandler(this.btnLoad_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
