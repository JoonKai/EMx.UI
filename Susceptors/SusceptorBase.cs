// Decompiled with JetBrains decompiler
// Type: EMx.UI.Susceptors.SusceptorBase
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using PH.DataTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EMx.UI.Susceptors
{
  [Serializable]
  public class SusceptorBase
  {
    public event WaferEventHandler WaferEvent;

    private void OnWaferEvent(
      object sender,
      SusceptorWaferViewerRaiseEventType type,
      string wafername)
    {
      WaferEventHandler waferEvent = this.WaferEvent;
      if (waferEvent == null)
        return;
      waferEvent(sender, type, wafername);
    }

    public event WaferEventHandler Click;

    private void OnClick(object sender, SusceptorWaferViewerRaiseEventType type, string wafername)
    {
      WaferEventHandler click = this.Click;
      if (click == null)
        return;
      click(sender, type, wafername);
    }

    public List<SingleCirclePositioner> WaferList { get; set; }

    public DTreeNode<SingleCirclePositioner> Tree { get; set; }

    public SusceptorUnit Unit { get; set; }

    public SusceptorBase()
    {
      this.Unit = SusceptorUnit.inch;
      this.WaferList = new List<SingleCirclePositioner>();
      this.Tree = new DTreeNode<SingleCirclePositioner>();
    }

    private void GenerateDummy()
    {
      this.WaferList = new List<SingleCirclePositioner>();
      this.Tree = new DTreeNode<SingleCirclePositioner>();
      SingleCirclePositioner circlePositioner1 = new SingleCirclePositioner();
      circlePositioner1.Radius = 500.0;
      this.Tree.Value = circlePositioner1;
      circlePositioner1.Position = new Point(circlePositioner1.Radius, circlePositioner1.Radius);
      circlePositioner1.StackedPosition = circlePositioner1.Position;
      circlePositioner1.PositionerType = SingleCirclePositionerType.Susceptor;
      this.WaferList.Add(circlePositioner1);
      SingleCirclePositioner circlePositioner2 = new SingleCirclePositioner();
      circlePositioner2.Radius = 300.0;
      circlePositioner2.DisplayCircle = false;
      circlePositioner2.PositionerType = SingleCirclePositionerType.Group;
      this.Tree.Nodes.Add(circlePositioner2);
      this.WaferList.Add(circlePositioner2);
      for (int index = 0; index < 12; ++index)
      {
        SingleCirclePositioner circlePositioner3 = new SingleCirclePositioner();
        circlePositioner3.Radius = 50.0;
        circlePositioner3.AngleOffset = (double) (index * 30);
        circlePositioner3.DistaceOffset = circlePositioner2.Radius;
        circlePositioner3.Angle = (double) (index * 30 - 90);
        this.FindNode(circlePositioner2).Nodes.Add(circlePositioner3);
        this.WaferList.Add(circlePositioner3);
      }
      SingleCirclePositioner circlePositioner4 = new SingleCirclePositioner();
      circlePositioner4.Radius = 400.0;
      circlePositioner4.DisplayCircle = false;
      circlePositioner4.PositionerType = SingleCirclePositionerType.Group;
      this.Tree.Nodes.Add(circlePositioner4);
      this.WaferList.Add(circlePositioner4);
      for (int index = 0; index < 12; ++index)
      {
        SingleCirclePositioner circlePositioner5 = new SingleCirclePositioner();
        circlePositioner5.Radius = 50.0;
        circlePositioner5.AngleOffset = (double) (index * 30);
        circlePositioner5.DistaceOffset = circlePositioner4.Radius;
        circlePositioner5.Angle = (double) (index * 30 - 90);
        this.FindNode(circlePositioner4).Nodes.Add(circlePositioner5);
        this.WaferList.Add(circlePositioner5);
      }
      SingleCirclePositioner circlePositioner6 = new SingleCirclePositioner();
      circlePositioner6.Radius = 100.0;
      circlePositioner6.DisplayCircle = false;
      circlePositioner6.PositionerType = SingleCirclePositionerType.Group;
      this.Tree.Nodes.Add(circlePositioner6);
      this.WaferList.Add(circlePositioner6);
      for (int index = 0; index < 4; ++index)
      {
        SingleCirclePositioner circlePositioner7 = new SingleCirclePositioner();
        circlePositioner7.Radius = 100.0;
        circlePositioner7.AngleOffset = (double) (index * 90);
        circlePositioner7.DistaceOffset = circlePositioner6.Radius;
        circlePositioner7.Angle = (double) (index * 90 - 90);
        this.FindNode(circlePositioner6).Nodes.Add(circlePositioner7);
        this.WaferList.Add(circlePositioner7);
      }
    }

    public double GetOuterWaferGroup()
    {
      if (!this.Tree.DepthFirstEnumerator.Any<SingleCirclePositioner>((Func<SingleCirclePositioner, bool>) (x => x.PositionerType == SingleCirclePositionerType.Group)))
        return 0.0;
      SingleCirclePositioner circlePositioner = this.Tree.DepthFirstEnumerator.Where<SingleCirclePositioner>((Func<SingleCirclePositioner, bool>) (item => item.PositionerType == SingleCirclePositionerType.Group)).OrderByDescending<SingleCirclePositioner, double>((Func<SingleCirclePositioner, double>) (item => item.Radius)).FirstOrDefault<SingleCirclePositioner>();
      return circlePositioner.DistaceOffset + circlePositioner.Radius;
    }

    internal void AddRoot(double waferSize)
    {
      SingleCirclePositioner circlePositioner = new SingleCirclePositioner();
      circlePositioner.Radius = waferSize;
      this.Tree.Value = circlePositioner;
      circlePositioner.Position = new Point(circlePositioner.Radius, circlePositioner.Radius);
      circlePositioner.StackedPosition = circlePositioner.Position;
      circlePositioner.PositionerType = SingleCirclePositionerType.Susceptor;
    }

    public void AddSubRingGroup(double wafersize, double initialAngle, bool isCCW)
    {
      double gap = 0.1;
      switch (this.Unit)
      {
        case SusceptorUnit.inch:
          gap = 0.1;
          break;
        case SusceptorUnit.mm:
          gap = 3.0;
          break;
      }
      if (this.WaferList.Count == 0)
      {
        this.GetOuterWaferGroup();
        int num1 = this.WaferList.Count > 0 ? this.WaferList.Max<SingleCirclePositioner>((Func<SingleCirclePositioner, int>) (x => x.Index)) : 0;
        SingleCirclePositioner circlePositioner1 = new SingleCirclePositioner();
        circlePositioner1.Radius = 0.0;
        circlePositioner1.DisplayCircle = false;
        circlePositioner1.PositionerType = SingleCirclePositionerType.Group;
        circlePositioner1.Index = num1 + 1;
        this.Tree.Nodes.Add(circlePositioner1);
        this.WaferList.Add(circlePositioner1);
        int num2 = 1;
        for (int index = 0; index < num2; ++index)
        {
          SingleCirclePositioner circlePositioner2 = new SingleCirclePositioner();
          circlePositioner2.Radius = wafersize / 2.0;
          circlePositioner2.AngleOffset = (double) index * 360.0 / (double) num2;
          circlePositioner2.DistaceOffset = circlePositioner1.Radius;
          circlePositioner2.Angle = (double) index * 360.0 / (double) num2;
          circlePositioner2.Index = index + num1 + 1;
          this.FindNode(circlePositioner1).Nodes.Add(circlePositioner2);
          this.WaferList.Add(circlePositioner2);
        }
      }
      else
      {
        double outerWaferGroup = this.GetOuterWaferGroup();
        int num = this.WaferList.Count > 0 ? this.WaferList.Max<SingleCirclePositioner>((Func<SingleCirclePositioner, int>) (x => x.Index)) : 0;
        SingleCirclePositioner circlePositioner3 = new SingleCirclePositioner();
        circlePositioner3.Radius = outerWaferGroup + wafersize + gap;
        circlePositioner3.DisplayCircle = false;
        circlePositioner3.PositionerType = SingleCirclePositionerType.Group;
        circlePositioner3.Angle = initialAngle;
        circlePositioner3.Index = num + 1;
        this.Tree.Nodes.Add(circlePositioner3);
        this.WaferList.Add(circlePositioner3);
        int waferCountInGroup = this.CalculateMaximumWaferCountInGroup(circlePositioner3.Radius, wafersize / 2.0, gap);
        for (int index = 0; index < waferCountInGroup; ++index)
        {
          SingleCirclePositioner circlePositioner4 = new SingleCirclePositioner();
          circlePositioner4.Radius = wafersize / 2.0;
          circlePositioner4.AngleOffset = (double) index * 360.0 / (double) waferCountInGroup * (isCCW ? 1.0 : -1.0) + initialAngle + 90.0;
          circlePositioner4.DistaceOffset = circlePositioner3.Radius;
          circlePositioner4.Angle = (double) index * 360.0 / (double) waferCountInGroup * (isCCW ? 1.0 : -1.0) + initialAngle;
          circlePositioner4.Index = index + num + 1;
          this.FindNode(circlePositioner3).Nodes.Add(circlePositioner4);
          this.WaferList.Add(circlePositioner4);
          circlePositioner4.WaferEvent += new WaferEventHandler(this.BaseCircle_WaferDrop);
        }
      }
    }

    internal void SortIndex()
    {
      List<SingleCirclePositioner> list = this.Tree.BreadthFirstEnumerator.Where<SingleCirclePositioner>((Func<SingleCirclePositioner, bool>) (x => x.PositionerType == SingleCirclePositionerType.Wafer)).ToList<SingleCirclePositioner>();
      for (int index = 0; index < list.Count; ++index)
        list[index].Index = index + 1;
      foreach (DTreeNode<SingleCirclePositioner> dtreeNode in this.Tree.BreadthFirstNodeEnumerator.Where<DTreeNode<SingleCirclePositioner>>((Func<DTreeNode<SingleCirclePositioner>, bool>) (x => x.Value.PositionerType == SingleCirclePositionerType.Group)).ToList<DTreeNode<SingleCirclePositioner>>())
      {
        IEnumerable<SingleCirclePositioner> breadthFirstEnumerator = dtreeNode.BreadthFirstEnumerator;
        if (breadthFirstEnumerator.Count<SingleCirclePositioner>() > 1)
          dtreeNode.Value.Index = breadthFirstEnumerator.ToList<SingleCirclePositioner>()[1].Index;
      }
    }

    internal void SelectRingGroup(int selectedIndex)
    {
      foreach (SingleCirclePositioner wafer in this.WaferList)
        wafer.DeSelect();
      SingleCirclePositioner circlePositioner1 = new SingleCirclePositioner();
      if (!this.TryGetRingGroup(selectedIndex, out circlePositioner1))
        return;
      foreach (SingleCirclePositioner circlePositioner2 in this.FindNode(circlePositioner1).BreadthFirstEnumerator)
        circlePositioner2.Select();
    }

    private void ModifySubring(CurrentRingInfo info, SingleCirclePositioner subring)
    {
      subring.Radius = info.Distance;
      subring.Angle = info.InitialAngle;
      int num = 0;
      DTreeNode<SingleCirclePositioner> node = this.FindNode(subring);
      List<DTreeNode<SingleCirclePositioner>> list = node.BreadthFirstNodeEnumerator.ToList<DTreeNode<SingleCirclePositioner>>();
      if (list.Count > 0)
        num = list.First<DTreeNode<SingleCirclePositioner>>().Value.Index - 1;
      foreach (DTreeNode<SingleCirclePositioner> dtreeNode in list)
      {
        DTreeNode<SingleCirclePositioner> ring = dtreeNode;
        this.WaferList.Remove(ring.Value);
        if (node.Nodes.Any<DTreeNode<SingleCirclePositioner>>((Func<DTreeNode<SingleCirclePositioner>, bool>) (X => X.Value == ring.Value)))
          node.Nodes.Remove(ring);
      }
      for (int index = 0; index < info.WaferCount; ++index)
      {
        SingleCirclePositioner circlePositioner = new SingleCirclePositioner();
        circlePositioner.Radius = info.WaferSize / 2.0;
        circlePositioner.AngleOffset = (double) index * 360.0 / (double) info.WaferCount * (info.IsCCW ? 1.0 : -1.0) + info.InitialAngle + 90.0;
        circlePositioner.DistaceOffset = subring.Radius;
        circlePositioner.Angle = (double) index * 360.0 / (double) info.WaferCount * (info.IsCCW ? 1.0 : -1.0) + info.InitialAngle;
        if (!info.IsIn)
          circlePositioner.Angle += 180.0;
        circlePositioner.Index = index + num + 1;
        this.FindNode(subring).Nodes.Add(circlePositioner);
        this.WaferList.Add(circlePositioner);
        circlePositioner.WaferEvent += new WaferEventHandler(this.BaseCircle_WaferDrop);
      }
    }

    internal void ModifyRingGroup(int selectedIndex, CurrentRingInfo info)
    {
      SingleCirclePositioner subring;
      if (!this.TryGetRingGroup(selectedIndex, out subring))
        return;
      this.ModifySubring(info, subring);
    }

    public void ConnectEvent() => this.WaferList.ForEach((Action<SingleCirclePositioner>) (x => x.WaferEvent += new WaferEventHandler(this.BaseCircle_WaferDrop)));

    private void BaseCircle_WaferDrop(
      object sender,
      SusceptorWaferViewerRaiseEventType type,
      string wafername)
    {
      this.OnWaferEvent(sender, type, wafername);
    }

    private void BaseCircle_Click(
      object sender,
      SusceptorWaferViewerRaiseEventType type,
      string wafername)
    {
      this.OnClick(sender, type, wafername);
    }

    public int CalculateMaximumWaferCountInGroup(
      double groupradius,
      double waferradius,
      double gap)
    {
      return (int) Math.Truncate(groupradius * 2.0 * Math.PI / (waferradius * 2.0 + gap));
    }

    public bool TryGetRingGroup(int index, out SingleCirclePositioner item)
    {
      List<DTreeNode<SingleCirclePositioner>> list = this.Tree.BreadthFirstNodeEnumerator.Where<DTreeNode<SingleCirclePositioner>>((Func<DTreeNode<SingleCirclePositioner>, bool>) (x => x.Value.PositionerType == SingleCirclePositionerType.Group)).ToList<DTreeNode<SingleCirclePositioner>>();
      if (list.Count > index && index >= 0)
      {
        item = list[index].Value;
        return true;
      }
      item = new SingleCirclePositioner();
      return false;
    }

    public CurrentRingInfo GetRingGroupInfo(int index)
    {
      SingleCirclePositioner circlePositioner = new SingleCirclePositioner();
      if (!this.TryGetRingGroup(index, out circlePositioner))
        return new CurrentRingInfo();
      DTreeNode<SingleCirclePositioner> node = this.FindNode(circlePositioner);
      CurrentRingInfo currentRingInfo = new CurrentRingInfo();
      currentRingInfo.WaferCount = node.Nodes.Count;
      if (currentRingInfo.WaferCount > 0)
        currentRingInfo.WaferSize = node.Nodes.First<DTreeNode<SingleCirclePositioner>>().Value.Radius * 2.0;
      currentRingInfo.Distance = node.Value.Radius;
      currentRingInfo.InitialAngle = node.Value.Angle;
      return currentRingInfo;
    }

    public DTreeNode<SingleCirclePositioner> FindNode(
      SingleCirclePositioner item)
    {
      if (this.Tree.Nodes.Any<DTreeNode<SingleCirclePositioner>>((Func<DTreeNode<SingleCirclePositioner>, bool>) (x => x.Value == item)))
        return this.Tree.Nodes.First<DTreeNode<SingleCirclePositioner>>((Func<DTreeNode<SingleCirclePositioner>, bool>) (x => x.Value == item));
      int num = (int) MessageBox.Show("노드없음");
      return (DTreeNode<SingleCirclePositioner>) null;
    }

    internal void GenerateDisplay(Size newSize)
    {
      foreach (DTreeNode<SingleCirclePositioner> dtreeNode in this.Tree.DepthFirstNodeEnumerator)
        this.GetParentsSize(dtreeNode);
    }

    public Point GetParentsSize(DTreeNode<SingleCirclePositioner> item)
    {
      if (item.Parent == null || item.IsRoot)
        return item.Value == null ? new Point() : item.Value.Position;
      Point parentsSize = this.GetParentsSize(item.Parent);
      item.Value.CalculatePosition();
      Point point = new Point(parentsSize.X + item.Value.Position.X, parentsSize.Y + item.Value.Position.Y);
      item.Value.StackedPosition = point;
      return point;
    }
  }
}
