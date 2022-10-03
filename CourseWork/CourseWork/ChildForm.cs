namespace CourseWork
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public partial class ChildForm : Form
    {
        private readonly BusinessLogic businessLogic = new BusinessLogic();

        /// <summary>
        /// Чи збережене зображення
        /// </summary>
        public bool IsSaved { get; private set; } = true;

        /// <summary>
        /// Інформація про зображення
        /// </summary>
        private ImageInformation ImageInformation => this.businessLogic.ImageInformation;

        /// <summary>
        /// Чи є зображення
        /// </summary>
        private bool HasImage { get; set; } = true;

        /// <summary>
        /// Координати попередньої точки
        /// </summary>
        private Point PreviousPoint { get; set; }

        /// <summary>
        /// Обраний колір
        /// </summary>
        private Color SelectedColor { get; set; } = Color.Black;

        /// <summary>
        /// Чи натиснука кнопка миші
        /// </summary>
        private bool IsClick { get; set; }

        public ChildForm()
        {
            this.InitializeComponent();
        }

        public ChildForm(Form container)
        {
            this.InitializeComponent();
            this.MdiParent = container;
            this.toolStripComboBox1.SelectedIndex = 0;
            this.pictureBox.Size = new Size(950, 650);
            this.pictureBox.Image = new Bitmap(this.pictureBox.Width, this.pictureBox.Height);
        }
        
        public ChildForm(Form container, string fileName)
        {
            this.InitializeComponent();
            this.MdiParent = container;
            this.toolStripComboBox1.SelectedIndex = 0;
            this.SetImage(fileName); // завантажуєжмо зображення по його шляху
        }

        private ChildForm(Form container, Image image)
        {
            this.InitializeComponent();
            this.MdiParent = container;
            this.toolStripComboBox1.SelectedIndex = 0;
            this.SetImage(image); // завантажуєжмо зображення
        }

        // встановлення зображення
        public void SetImage(string fileName)
        {
            var image = Image.FromFile(fileName); // завантажуємо
            var name = Path.GetFileName(fileName); // отримуємо ім'я
            this.pictureBox.Image = image; // виводимо в pictureBox
            this.Text = name; // встановлюємо заголовок
            this.businessLogic.AddImageInformation(name, fileName, image); // додаємо параметри зображення
            this.HasImage = true;
        }

        // збереження зображення
        public void Save(string fileName = null)
        {
            var bitmap = (Bitmap)this.pictureBox.Image;

            // якщо є ім'я
            if (fileName != null)
            {
                bitmap.Save(fileName);
                this.IsSaved = true;
            }
            else
            {
                // якщо не має то отримуємо ім'я з saveFileDialog
                if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bitmap.Save(this.saveFileDialog.FileName);
                    fileName = Path.GetFileName(this.saveFileDialog.FileName);

                    // додаємо параметри
                    this.businessLogic.AddImageInformation(fileName, this.saveFileDialog.FileName, bitmap);
                    this.IsSaved = true;
                }
            }

            this.Text = Path.GetFileName(fileName);
        }

        // встановлення зображення
        private void SetImage(Image image)
        {
            this.pictureBox.Image = image;
            this.HasImage = true;
            this.IsSaved = false;
        }

        // подія закриття форми
        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // якщо зображення не збережено то пропонуємо зберегти
            if (!this.IsSaved)
            {
                switch (MessageBox.Show(
                    $@"Зображення {this.Text} не збереженно! Зберегти?",
                    @"Повідомлення",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        this.Save();
                        this.Hide();
                        this.Close();
                        break;

                    case DialogResult.No:
                        this.Hide();
                        this.Close();
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }         
        }

        // пунк меню інформація про зображення
        private void InformationAboutImageToolStripMenuItemMouseEnter(object sender, EventArgs e)
        {
            var imageInformation = this.ImageInformation;
            MessageBox.Show(imageInformation.ToString(), $@"Інформація про файл {imageInformation.Name}", MessageBoxButtons.OK);
        }

        // клік на інформація для задання активності кнопки "інформація про зображення"
        private void InformationToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.informationAboutImageToolStripMenuItem.Enabled = this.HasImage && this.IsSaved;
        }

        // клік на функції для заданян активності "додати зображення"
        private void FunctionsToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.differenceImagesToolStripMenuItem.Enabled = this.HasImage && this.IsSaved;
        }

        // натиснення миші на PictureBox
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            this.IsClick = true;
            this.PreviousPoint = e.Location;
        }

        // рух миші на PictureBox
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsClick)
            {
                // якщо вибраний олівець
                if (this.pensilToolStripButton.Checked)
                {
                    // створюємо нову бітову карту
                    var bitmap = new Bitmap(this.pictureBox.Image);

                    // створюємо перо
                    using (var pen = new Pen(this.SelectedColor, float.Parse(this.toolStripComboBox1.Text)))

                    // отримуємо контекст для малювання
                    using (var grafics = Graphics.FromImage(bitmap))
                    {
                        // малюємо лінію
                        grafics.DrawLine(pen, this.PreviousPoint, e.Location);

                        grafics.Save(); // зберігаємо
                        this.pictureBox.Image = bitmap; // ставимо нове зображення
                    }

                    // зображення не збережено
                    this.IsSaved = false;
                }

                // якщо обрано ластик
                if (this.eraserToolStripButton.Checked)
                {
                    // радіус щоб квадрат був по центру
                    var radius = int.Parse(this.toolStripComboBox1.Text) / 2;

                    // створюємо прямокутник
                    var rectangle = new Rectangle(e.X - radius, e.Y - radius, 2 * radius, 2 * radius);

                    // створюємо нову бітову карту
                    var bitmap = new Bitmap(this.pictureBox.Image);

                    // створюємо перо
                    using (var pen = new Pen(Color.White, float.Parse(this.toolStripComboBox1.Text)))

                        // отримуємо контекст для малювання
                    using (var grafics = Graphics.FromImage(bitmap))
                    {
                        // малюємо прямокутник
                        grafics.DrawRectangle(pen, rectangle);

                        grafics.Save(); // зберігаємо
                        this.pictureBox.Image = bitmap; // ставимо нове зображення
                    }

                    // зображення не збережено
                    this.IsSaved = false;
                }

                this.PreviousPoint = e.Location;
            }
        }
        
        // відпущення миші на PictureBox
        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            this.IsClick = false;
        }

        // різниця зображень
        private void DifferenceImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Image resultImage;
                try
                {
                    this.businessLogic.ImAbsDiff(this.pictureBox.Image, Image.FromFile(this.openFileDialog.FileName), out resultImage);

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, @"Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var childForm = new ChildForm(this.MdiParent, resultImage)
                                    {
                                        Text = $@"Різниця {this.Text} та {Path.GetFileName(this.openFileDialog.FileName)}",
                                        WindowState = FormWindowState.Maximized
                                    };
                childForm.Show();
            }
        }

        // пукт меню олівець
        private void PensilToolStripButton_Click(object sender, EventArgs e)
        {
            this.pensilToolStripButton.Checked = !this.pensilToolStripButton.Checked;
            this.eraserToolStripButton.Checked = false;

            // в ms paint такі розміри олівця: 1, 2, 3, 4
            // поставимо їх
            this.toolStripComboBox1.Items.Clear();
            this.toolStripComboBox1.Items.AddRange(new object[] { 1, 2, 3, 4 });
            this.toolStripComboBox1.SelectedIndex = 0;
        }

        // пукт меню ластик
        private void EraserToolStripButton_Click(object sender, EventArgs e)
        {
            this.eraserToolStripButton.Checked = !this.eraserToolStripButton.Checked;
            this.pensilToolStripButton.Checked = false;

            // в ms paint такі розміри ластику: 4, 6, 8, 10
            this.toolStripComboBox1.Items.Clear();
            this.toolStripComboBox1.Items.AddRange(new object[] { 4, 6, 8, 10 });
            this.toolStripComboBox1.SelectedIndex = 0;
        }

        // пукт меню вибір кольору
        private void ChooseColorToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.SelectedColor = this.colorDialog1.Color;
            }
        }
    }
}