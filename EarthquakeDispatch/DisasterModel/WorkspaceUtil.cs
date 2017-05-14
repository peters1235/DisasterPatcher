using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;

namespace DisasterModel
{
    class WorkspaceUtil
    {
        public static IFeatureWorkspace OpenShapeWorkspace(string strShapeName)
        {
            // As Workspace Factories are Singleton objects, they must be instantiated with the Activator
            var workspaceFactory = System.Activator.CreateInstance(System.Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory")) as ESRI.ArcGIS.Geodatabase.IWorkspaceFactory;

            if (!System.IO.Directory.Exists(strShapeName))
            {
                MessageBox.Show("The workspace: " + strShapeName + " does not exist", "Workspace Error");
                return null;
            }

            IWorkspace workspace = null;
            try
            {
                workspace = workspaceFactory.OpenFromFile(strShapeName, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Opening workspace failed: " + ex.Message, "Workspace Error");
            }

            return workspace as IFeatureWorkspace;
        }

        internal static IFeatureWorkspace OpenMDBWorkspace(string wsPath)
        {
            var workspaceFactory = System.Activator.CreateInstance(System.Type.GetTypeFromProgID("esriDataSourcesGDB.AccessWorkspaceFactory")) as ESRI.ArcGIS.Geodatabase.IWorkspaceFactory;

            if (!System.IO.File.Exists(wsPath))
            {
                MessageBox.Show("The workspace: " + wsPath + " does not exist", "Workspace Error");
                return null;
            }

            IWorkspace workspace = null;
            try
            {
                workspace = workspaceFactory.OpenFromFile(wsPath, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Opening workspace failed: " + ex.Message, "Workspace Error");
            }

            return workspace as IFeatureWorkspace;
        }
    }
}
