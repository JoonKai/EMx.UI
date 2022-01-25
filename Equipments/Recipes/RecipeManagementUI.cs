// Decompiled with JetBrains decompiler
// Type: EMx.UI.Equipments.Recipes.RecipeManagementUI
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Engine.Linkers;
using EMx.Equipments.Configurations;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace EMx.UI.Equipments.Recipes
{
  [InstanceContract(ClassID = "742b6bd7-7efa-4cc9-bad1-609ec1d5dfcd")]
  public class RecipeManagementUI : IManagedType
  {
    private static ILog log = LogManager.GetLogger();
    private string LastUsedRecipePath = "";

    [InstanceMember]
    [GridViewItem(true)]
    [Category("Recipe")]
    public virtual string FileExtension { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("Recipe")]
    public virtual bool IncludeSubDirectoryFiles { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [DesignableMember(true)]
    [Category("Recipe")]
    public virtual string BaseDirectory { get; set; }

    [DesignableMember(true)]
    [Category("Recipe")]
    [DeclaredLinkedState(eDeclaredLinkedState.Target)]
    public virtual ConfigTemplateStructure RecipeRoot { get; set; }

    public RecipeManagementUI()
    {
      this.FileExtension = "recipe";
      this.IncludeSubDirectoryFiles = true;
      this.BaseDirectory = "c:\\bitplex\\recipe";
    }

    private Window CurrentWindow => Application.Current.MainWindow;

    [QueryableMember(true)]
    public void LoadRecipe_Clicked()
    {
      string filepath = this.CurrentWindow.OpenFileDialog(this.BaseDirectory, string.Format("Recipe File(*.{0})|*.{0}", (object) this.FileExtension), this.FileExtension);
      if (string.IsNullOrEmpty(filepath))
        return;
      Tuple<object, List<string>> tuple = InstanceSerializer.ReadFromFile(filepath);
      ConfigData conf = tuple.Item1 as ConfigData;
      if (tuple.Item2.Count > 0)
      {
        int num1 = (int) this.CurrentWindow.ShowWarningMessage("Recipe Manager", string.Join(Environment.NewLine, (IEnumerable<string>) tuple.Item2), width: 900, height: 600);
      }
      if (conf != null && this.RecipeRoot != null)
      {
        if (!new ConfigHelper().Deserialize(conf, this.RecipeRoot))
        {
          int num2 = (int) this.CurrentWindow.ShowWarningMessage("Recipe Manager", "Deserialization Failed.");
        }
        else
          this.LastUsedRecipePath = filepath;
      }
    }

    [QueryableMember(true)]
    public void SaveRecipe_Clicked()
    {
      if (string.IsNullOrWhiteSpace(this.LastUsedRecipePath) || !File.Exists(this.LastUsedRecipePath))
        this.SaveAsRecipe_Clicked();
      else if (this.SaveRecipe(this.LastUsedRecipePath, this.RecipeRoot))
      {
        int num1 = (int) this.CurrentWindow.ShowNormalMessage("Recipe Manager", "Save Completed\r\n" + this.LastUsedRecipePath);
      }
      else
      {
        int num2 = (int) this.CurrentWindow.ShowWarningMessage("Recipe Manager", "Failed to save the recipe.\r\nPlease check the program log files.\r\n" + this.LastUsedRecipePath);
      }
    }

    [QueryableMember(true)]
    public void SaveAsRecipe_Clicked()
    {
      string path = this.CurrentWindow.SaveFileDialog(this.BaseDirectory, string.Format("Recipe File(*.{0})|*.{0}", (object) this.FileExtension), this.FileExtension);
      if (string.IsNullOrEmpty(path))
        return;
      if (this.SaveRecipe(path, this.RecipeRoot))
      {
        this.LastUsedRecipePath = path;
        int num = (int) this.CurrentWindow.ShowNormalMessage("Recipe Manager", "Save Completed\r\n" + this.LastUsedRecipePath);
      }
      else
      {
        int num1 = (int) this.CurrentWindow.ShowWarningMessage("Recipe Manager", "Failed to save the recipe.\r\nPlease check the program log files.\r\n" + this.LastUsedRecipePath);
      }
    }

    private bool SaveRecipe(string path, ConfigTemplateStructure templates)
    {
      if (string.IsNullOrEmpty(path))
        return false;
      ConfigData conf = new ConfigData();
      if (File.Exists(path))
      {
        Tuple<object, List<string>> tuple = InstanceSerializer.ReadFromFile(path);
        if (tuple.Item1 is ConfigData configData2)
          conf = configData2;
        List<string> stringList = tuple.Item2;
        if (stringList.Count > 0)
        {
          int num = (int) this.CurrentWindow.ShowWarningMessage("Recipe Manager (On Load)", string.Join(Environment.NewLine, (IEnumerable<string>) stringList));
          return false;
        }
      }
      ConfigHelper configHelper = new ConfigHelper();
      conf.ConfigCategory = "Recipe";
      if (!configHelper.Serialize(conf, templates))
      {
        int num = (int) this.CurrentWindow.ShowWarningMessage("Recipe Manager", "Serialization failed.");
        return false;
      }
      Path.GetFileNameWithoutExtension(path);
      List<string> file = InstanceSerializer.WriteToFile(path, (object) conf);
      if (file.Count <= 0)
        return true;
      int num1 = (int) this.CurrentWindow.ShowWarningMessage("Recipe Manager (On Save)", string.Join(Environment.NewLine, (IEnumerable<string>) file));
      return false;
    }

    [QueryableMember(true)]
    public void DeleteRecipe_Clicked()
    {
      string path = this.CurrentWindow.OpenFileDialog(this.BaseDirectory, string.Format("Recipe File(*.{0})|*.{0}", (object) this.FileExtension), this.FileExtension);
      if (string.IsNullOrEmpty(path) || !File.Exists(path))
        return;
      RecipeManagementUI.log.Info("Delete Recipe : {0}", (object) path);
      File.Delete(path);
    }
  }
}
