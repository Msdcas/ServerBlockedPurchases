using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerBlockedPurchases.Classes
{
    internal static class TableHandler
    {
        public static DataGridView dataGrid;
        public static void SetDataGridView(ref DataGridView dataGridView)
        {
            dataGrid = dataGridView;
        }

        private delegate void DelegLog(User User);

        public static void DelRowByUserName(User User)
        {
            if (dataGrid.InvokeRequired)
            {
                DelegLog d = new DelegLog(DelRowByUserName);
                dataGrid.Invoke(d, new object[] { User });
            }
            else
            {
                if (User != null)
                {
                    int index = IndexRowByIpAddress(User.IpAddress);
                    if (index != -1)
                        lock (dataGrid)
                            dataGrid.Rows.RemoveAt(index);
                }
            }
                
        }

        public static void SetIndexEditedRowByUserName(User User)
        {
            if (dataGrid.InvokeRequired)
            {
                DelegLog d = new DelegLog(SetIndexEditedRowByUserName);
                dataGrid.Invoke(d, new object[] { User });
            }
            else
            {
                int indTableRow = IndexRowByIpAddress(User.IpAddress);
                if (indTableRow != -1)
                {
                    dataGrid.Rows[indTableRow].Cells[0].Value = User.IndexEditedSqlRow;
                    dataGrid.Rows[indTableRow].Cells[1].Value = User.dateTime.ToString("dd-MM-yyyy hh:mm:ss");
                }
                else
                {
                    AddRow(User);
                }
            }
        }

        private static void AddRow(User User)
        {
            if (dataGrid.InvokeRequired)
            {
                DelegLog d = new DelegLog(AddRow);
                dataGrid.Invoke(d, new object[] { User });
            }
            else
            {
                lock(dataGrid)
                    dataGrid.Rows.Add();
                int indexLastRow = dataGrid.Rows.Count - 1;
                
                dataGrid.Rows[indexLastRow].Cells[0].Value = User.IndexEditedSqlRow;
                dataGrid.Rows[indexLastRow].Cells[1].Value = User.dateTime.ToString("dd-MM-yyyy hh:mm:ss");
                dataGrid.Rows[indexLastRow].Cells[2].Value = User.FullName;
                dataGrid.Rows[indexLastRow].Cells[3].Value = User.IdDepart;
                dataGrid.Rows[indexLastRow].Cells[4].Value = User.IpAddress;
                
            }
        }     

        private static int IndexRowByIpAddress(string IpAddress)
        {
            for (int i = 0; i < dataGrid.Rows.Count; i++)
                if (dataGrid.Rows[i].Cells[4].Value.ToString() == IpAddress)
                    return i;
            return -1;
        }

    }
}
