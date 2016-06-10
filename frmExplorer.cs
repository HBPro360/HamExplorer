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
            //e.Node.Text;
            DirectoryInfo di = new DirectoryInfo(e.Node.Text);
            DirectoryInfo[] directories = di.GetDirectories();
            this.lV.Items.Clear();
            foreach(DirectoryInfo d in directories)
            {
                ListViewItem li = new ListViewItem(d.Name);
                this.lV.Items.Add(li);
            }
        }
    }
}
