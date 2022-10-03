namespace CourseWork
{
    using System;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            this.InitializeComponent();
        }

        // пункт меню створити
        private void CreateToolStripMenuItemClick(object sender, EventArgs e)
        {
            // створюємо нове вікно ChildForm
            var childForm = new ChildForm(this) { WindowState = FormWindowState.Maximized };
            childForm.Show();
        }

        // пункт меню відкрити
        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            // показуємо openFileDialog для відриття файлі і якщо натиснули ОК то продовжуємо
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // якщо немає ChildForm
                if (this.ActiveMdiChild == null)
                {
                    // створюємо ChildForm, передаючи в нього повне ім'я файлу для відкриття
                    var childForm = new ChildForm(this, this.openFileDialog.FileName)
                                        {
                                            WindowState = FormWindowState.Maximized
                                        };
                    childForm.Show();
                }
                else if (this.ActiveMdiChild is ChildForm childForm)
                {
                    // якщо вікно уже створено, то викликаємо ф-ю для додвання зображення
                    childForm.SetImage(this.openFileDialog.FileName);                  
                }
            }
        }

        // пункт меню зберегти
        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            // якщо є дочірне вікно
            if (this.ActiveMdiChild != null)
            {
                // якщо дочірне віко ChildForm
                if (this.ActiveMdiChild is ChildForm childForm)
                {
                    // якщо зображення не збережене
                    if (!childForm.IsSaved)
                    {
                        childForm.Save(); // викликаємо ф-ю для збереження
                    }
                }
            }
        }

        // пункт меню зберегти як
        private void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
            {
                if (this.ActiveMdiChild is ChildForm childForm)
                {
                    // встановлюємо ім'я файлу як ім'я дочерного вікна
                    this.saveFileDialog.FileName = childForm.Text;

                    // показуємо saveFileDialog
                    if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var fileName = this.saveFileDialog.FileName;
                        childForm.Save(fileName); // викликаємо ф-ю для збереження
                    }
                }
            }
        }

        // пункт меню закрити
        private void CloseToolStripMenuItemClick(object sender, EventArgs e)
        {
            // якщо є активне вікно і воно childForm
            if (this.ActiveMdiChild != null && this.ActiveMdiChild is ChildForm childForm)
            {
                childForm.Close(); // закриваємо це активне вікно
            }
        }

        // пункт меню вихід
        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.Close(); // закриваємо головне вікно
        }

        // якщо мишка знаходиться на "Файл" (подія MouseEnter)
        private void FileToolStripMenuItemMouseEnter(object sender, EventArgs e)
        {
            // встановлюємо доступність пунктів меню
            this.відкритиToolStripMenuItem.Visible = true;
            this.saveToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Enabled = false;
            this.closeToolStripMenuItem.Enabled = false;

            // якщо є дочірне вікно
            if (this.ActiveMdiChild != null)
            {
                this.closeToolStripMenuItem.Enabled = true;
                this.saveAsToolStripMenuItem.Enabled = true;

                // якщо не збережене зображення
                if (this.ActiveMdiChild is ChildForm childForm && !childForm.IsSaved)
                {
                    this.saveToolStripMenuItem.Enabled = true;
                }
            }
        }

        // при активації дочірнього вікна переносимо з нього паннель інструментів на головне віко
        private void MainFormMdiChildActivate(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge(this.toolStrip1);

            if (this.ActiveMdiChild is ChildForm frmChild)
            {
                ToolStripManager.Merge(frmChild.toolStrip1, this.toolStrip1); // об'єднання
                this.toolStrip1.Visible = true;
            }
        }
    }
}