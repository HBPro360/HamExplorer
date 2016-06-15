using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HamExplorer
{
    public partial class frmExplorer : Form
    {
        TreeNode tNodeRt;
        public frmExplorer()
        {
            InitializeComponent();
            this.tV.NodeMouseClick += TV_NodeMouseClick;
            lV.MouseDoubleClick += LV_MouseDoubleClick;
        }
        private void LV_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lV.SelectedItems.Count == 1)
            {
                //string item = lV.SelectedItems[0].Text;
                GetDirectories(lV.SelectedItems[0]);
            }
        }

        private void frmExplorer_Load(object sender, EventArgs e)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            tNodeRt = new TreeNode("Computer");
            this.tV.Nodes.Add(tNodeRt);
            foreach (DriveInfo di in allDrives)
            {
                tNodeRt.Nodes.Add(di.Name);
            }
            this.tV.ExpandAll();

        }
        private void TV_NodeMouseClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            lV.View = View.Details;
            if (e.Node.Text == "Computer")
            {
                lV.Items.Clear();
                lV.View = View.LargeIcon;
                lV.LargeImageList = iLIcon;
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo di in allDrives)
                {
                    ListViewItem lvi = null;
                    if (di.DriveType == DriveType.Fixed)
                    {
                        lvi = new ListViewItem(di.Name, (int)Drives.LocalDrive);
                    }
                    else if (di.DriveType == DriveType.CDRom)
                    {
                        lvi = new ListViewItem(di.Name, (int)Drives.DVD);
                    }
                    else if (di.DriveType == DriveType.Network)
                    {
                        lvi = new ListViewItem(di.Name, (int)Drives.NetworkDrive);
                    }
                    else if (di.DriveType == DriveType.Removable)
                    {
                        lvi = new ListViewItem(di.Name, (int)Drives.FlashDrives);
                    }
                    lV.Items.Add(lvi);
                }
            }
            else
            {
                try
                {
                    GetDirectories(e.Node.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!");
                }
            }
        }
        //end of node mouse click
        private void GetDirectories(ListViewItem item)
        {
            DirectoryInfo[] directories;
            string path = Path.Combine(item.Tag.ToString(), item.Text);
            cmbBox.Text = path;
            DirectoryInfo di = new DirectoryInfo(path);
            directories = di.GetDirectories();
            this.lV.Items.Clear();
            foreach (DirectoryInfo d in directories)
            {
                if (!d.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    ListViewItem li = new ListViewItem(d.Name, 0);
                    li.Tag = path;                    
                    this.lV.Items.Add(li);
                    lV.SmallImageList = iLDetail;
                }
            }
            GetFiles(path);
        }
        private void GetDirectories(string drive)
        {
            DirectoryInfo[] directories;
            DirectoryInfo di = new DirectoryInfo(drive);
            cmbBox.Text = drive;
            directories = di.GetDirectories();
            this.lV.Items.Clear();
            foreach (DirectoryInfo d in directories)
            {
                if (!d.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    ListViewItem li = new ListViewItem(d.Name, 0);
                    li.Tag = drive;                    
                    this.lV.Items.Add(li);
                    lV.SmallImageList = iLDetail;
                }
            }
            GetFiles(drive);
        }
        public void GetFiles(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            FileInfo[] files = di.GetFiles();
            foreach(FileInfo fi in files)
            {
                ListViewItem li = new ListViewItem(fi.Name);
                li.Tag = fi.FullName;
                lV.Items.Add(li);
            }
        }
    }

    public enum Drives
    {
        LocalDrive = 0,
        DVD = 1,
        NetworkDrive = 2,
        FlashDrives = 3
    }
    public enum Folders
    {

    }
}
