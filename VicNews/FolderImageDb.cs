using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Text.RegularExpressions;
using ShareLib;

public class FolderImageDb
{
    private FolderImageSource _source;

    public FolderImageDb(string Folder, string FileAttr, int PageCount)
    {
        _source = new FolderImageSource("Image", "Title", "Index ASC", "Page", Folder, FileAttr, PageCount);
        _source.AddFileToRow += AddFolderImage;
        _source.SortFiles += SortFiles;
        _source.LoadFiles();
    }

    public int PageNumber
    {
        get { return MyFunc.GetSessionData("FolderImage_PageNumber", 1); }
    }

    public DataTable GetPagingTable()
    {
        return _source.CreatePagingTable();
    }

    public int PageTotal()
    {
        return _source.PageTotal();
    }

    public DataTable GotoPage(int Number)
    {
        MyFunc.SaveSessionData("FolderImage_PageNumber", Number);
        return _source.GotoPage(Number);
    }

    private void AddFolderImage(DataRow Row, FileInfo Info, object Data)
    {
        Row["Image"] = Data.ToString() + "/" + Info.Name;
        Row["Title"] = ExtractTitle(Info.Name);
    }

    private void SortFiles(object sender, EventArgs e)
    {
        FileInfo[] fi = sender as FileInfo[];
        Array.Sort(fi, delegate(FileInfo f1, FileInfo f2)
        {
            int n1 = ExtractTitleNo(f1.Name);
            int n2 = ExtractTitleNo(f2.Name);
            return n1 - n2;
        });
    }

    private string ExtractTitle(string Text)
    {
        /*     Regex rgx = new Regex("^[\\d]+\\)");
             Text = rgx.Replace(Text, "");
             rgx = new Regex("^[\\d]+）");
             Text = rgx.Replace(Text, "");*/
        Regex rgx = new Regex("(_resize)*.JPG$", RegexOptions.IgnoreCase);
        Text = rgx.Replace(Text, "");
        return Text;
    }

    private int ExtractTitleNo(string Text)
    {
        Regex rgx = new Regex("^[\\d]+");
        Match m = rgx.Match(Text);
        if (m.Success)
            return Convert.ToInt32(m.Value);
        return 1000;
    }

}
