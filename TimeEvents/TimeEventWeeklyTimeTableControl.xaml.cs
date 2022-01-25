// Decompiled with JetBrains decompiler
// Type: EMx.UI.TimeEvents.TimeEventWeeklyTimeTableControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Serialization;
using EMx.TimeEvents;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.TimeEvents
{
  [InstanceContract(ClassID = "c4d96f0e-90dc-4a2d-806a-1ed676a0ab88")]
  public partial class TimeEventWeeklyTimeTableControl : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty EventManagerProperty = DependencyProperty.Register(nameof (EventManager), typeof (TimeEventManager), typeof (TimeEventWeeklyTimeTableControl));
    private bool _contentLoaded;

    [DesignableMember(true)]
    public TimeEventManager EventManager
    {
      get => (TimeEventManager) this.GetValue(TimeEventWeeklyTimeTableControl.EventManagerProperty);
      set => this.SetValue(TimeEventWeeklyTimeTableControl.EventManagerProperty, (object) value);
    }

    [DesignableMember(true)]
    public WeeklyTimeTableOptions Options { get; set; }

    protected List<DailyTimeEventItemLocation> Locations { get; set; }

    public event Action<object, ITimeEvent> ItemDoubleClickEvent;

    public TimeEventWeeklyTimeTableControl()
    {
      this.InitializeComponent();
      RenderOptions.SetEdgeMode((DependencyObject) this, EdgeMode.Aliased);
      RenderOptions.SetBitmapScalingMode((DependencyObject) this, BitmapScalingMode.NearestNeighbor);
      this.Locations = new List<DailyTimeEventItemLocation>();
      this.Options = new WeeklyTimeTableOptions();
      this.EventManager = new TimeEventManager();
    }

    protected override void OnRender(DrawingContext dc)
    {
      base.OnRender(dc);
      List<eDayOfWeek> viewDays = this.Options.GetViewDays();
      if (viewDays.Count == 0)
        return;
      int num1 = 0;
      int num2 = 0;
      double num3 = this.ActualWidth - (double) num1 - 1.0;
      double num4 = this.ActualHeight - (double) num2 - 1.0;
      int num5 = viewDays.Count + 1;
      int num6 = 35;
      double num7 = num3 / (double) num5;
      int num8 = 5;
      List<int> source1 = new List<int>();
      for (int index = 0; index <= num5; ++index)
      {
        double num9 = (double) num1 + num7 * (double) index;
        source1.Add((int) num9);
      }
      double num10 = Math.Round((double) (this.Options.EndHour - this.Options.BeginHour) / this.Options.StepHour);
      double num11 = (num4 - (double) num6) / num10;
      double num12 = num7;
      double num13 = num11 / this.Options.StepHour;
      List<int> source2 = new List<int>() { num2 };
      for (int index = 0; (double) index <= num10; ++index)
      {
        double num14 = (double) (num2 + num6) + num11 * (double) index;
        source2.Add((int) num14);
      }
      if (num12 <= 0.0 || num11 <= 0.0 || source2.Count == 0 || source1.Count == 0)
        return;
      Brush brush1 = "#B9CDE5".ToBrush();
      "#B9CDE5".ToBrush();
      Pen pen = new Pen("#1F2226".ToBrush(), 1.0);
      List<Brush> list1 = new List<string>()
      {
        "#C0E068",
        "#98C800",
        "#80B000",
        "#F8D880",
        "#F8B820",
        "#F8A010",
        "#F89090",
        "#F84040",
        "#E82020"
      }.Select<string, Brush>((Func<string, Brush>) (x => x.ToBrush())).ToList<Brush>();
      int num15 = source2.First<int>();
      int num16 = source2.Last<int>();
      int num17 = source1.First<int>();
      int num18 = source1.Last<int>();
      for (int index = 0; index < source1.Count; ++index)
      {
        int num19 = source1[index];
        dc.DrawLine(pen, new Point((double) num19, (double) num15), new Point((double) num19, (double) num16));
      }
      for (int index = 0; index < source2.Count; ++index)
      {
        int num20 = source2[index];
        dc.DrawLine(pen, new Point((double) num17, (double) num20), new Point((double) num18, (double) num20));
      }
      for (int index = 0; index < viewDays.Count; ++index)
      {
        eDayOfWeek eDayOfWeek = viewDays[index];
        int num21 = source1[index + 1];
        int num22 = source1[index + 2] - num21;
        dc.DrawRectangle(brush1, pen, new Rect((double) num21, (double) num15, (double) num22, (double) num6));
        FormattedText formattedText = new FormattedText(eDayOfWeek.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), this.FontSize, (Brush) Brushes.Black);
        dc.DrawText(formattedText, new Point((double) num21 + ((double) num22 - formattedText.Width) / 2.0, (double) num15 + ((double) num6 - formattedText.Height) / 2.0));
      }
      for (int index = 0; (double) index < num10; ++index)
      {
        double num23 = (double) this.Options.BeginHour + this.Options.StepHour * (double) index;
        double num24 = (double) this.Options.BeginHour + this.Options.StepHour * (double) (index + 1);
        int num25 = source2[index + 1];
        int num26 = source2[index + 2] - num25;
        int num27 = source1[0];
        int num28 = source1[1];
        dc.DrawRectangle(brush1, pen, new Rect((double) num27, (double) num25, (double) num28, (double) num26));
        FormattedText formattedText = new FormattedText(string.Format("{0:00}:{1:00} - {2:00}:{3:00}", (object) (int) num23, (object) (num23 % 1.0 * 60.0), (object) (int) num24, (object) (num24 % 1.0 * 60.0)), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 10.0, (Brush) Brushes.Black);
        dc.DrawText(formattedText, new Point((double) num27 + ((double) num28 - formattedText.Width) / 2.0, (double) num25 + ((double) num26 - formattedText.Height) / 2.0));
      }
      List<string> stringList = new List<string>();
      this.Locations.Clear();
      TimeEventManager eventManager = this.EventManager;
      for (int index1 = 0; index1 < viewDays.Count; ++index1)
      {
        eDayOfWeek d = viewDays[index1];
        List<DailyTimeEvent> list2 = eventManager.ConvertToDailyEvent(d, (Func<ITimeEvent, bool>) (x => x.IsInclusive(d))).ToList<DailyTimeEvent>();
        list2.Reverse();
        int num29 = source1[index1 + 1];
        int num30 = source1[index1 + 2] - num29;
        if (num30 > 0)
        {
          int beginHour = this.Options.BeginHour;
          int endHour = this.Options.EndHour;
          for (int index2 = 0; index2 < list2.Count; ++index2)
          {
            DailyTimeEvent dailyTimeEvent = list2[index2];
            TimeSpan startTime = dailyTimeEvent.StartTime;
            TimeSpan endTime = dailyTimeEvent.EndTime;
            int num31 = source2[1] + (int) (Math.Max((double) beginHour, Math.Min((double) endHour, startTime.TotalHours)) * num13);
            int num32 = source2[1] + (int) (Math.Max((double) beginHour, Math.Min((double) endHour, endTime.TotalHours)) * num13) - num31;
            if (num32 > 0)
            {
              if (!stringList.Contains(dailyTimeEvent.EventID))
                stringList.Add(dailyTimeEvent.EventID);
              int num33 = num30 - dailyTimeEvent.LowerCollisionNumber * num8;
              int num34 = num29 + dailyTimeEvent.LowerCollisionNumber * num8;
              Brush brush2 = list1[stringList.IndexOf(dailyTimeEvent.EventID) % list1.Count];
              Rect rect = new Rect((double) num34, (double) num31, (double) num33, (double) num32);
              dc.DrawRectangle(brush2, pen, rect);
              FormattedText formattedText = new FormattedText(dailyTimeEvent.EventName ?? "", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 10.0, (Brush) Brushes.Black);
              formattedText.MaxTextWidth = (double) (num33 - 5);
              dc.DrawText(formattedText, new Point((double) num34 + ((double) num33 - formattedText.Width) / 2.0, (double) num31 + ((double) num32 - formattedText.Height) / 2.0));
              this.Locations.Add(new DailyTimeEventItemLocation(dailyTimeEvent.OriginalSource, rect));
            }
          }
        }
      }
    }

    private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      Point pt = e.GetPosition((IInputElement) this);
      lock (this.Locations)
      {
        DailyTimeEventItemLocation eventItemLocation = this.Locations.LastOrDefault<DailyTimeEventItemLocation>((Func<DailyTimeEventItemLocation, bool>) (x =>
        {
          if (x.TimeEvent == null)
            return false;
          Rect location = x.Location;
          return x.Location.Contains(pt);
        }));
        if (eventItemLocation == null)
          return;
        Action<object, ITimeEvent> doubleClickEvent = this.ItemDoubleClickEvent;
        if (doubleClickEvent != null)
          doubleClickEvent((object) this, eventItemLocation.TimeEvent);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/timeevents/timeeventweeklytimetablecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        ((Control) target).MouseDoubleClick += new MouseButtonEventHandler(this.UserControl_MouseDoubleClick);
      else
        this._contentLoaded = true;
    }
  }
}
