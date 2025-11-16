using System;
using System.IO;
using System.Windows.Forms;

namespace Notepad
{
    public partial class MainForm : Form
    {
        private string currentFilePath = null;
        private bool isModified = false;

        public MainForm()
        {
            InitializeComponent();
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            string fileName = currentFilePath != null 
                ? Path.GetFileName(currentFilePath) 
                : "Yeni Dosya";
            
            if (isModified)
                fileName += "*";
            
            this.Text = $"{fileName} - Notepad";
        }

        private void NewFile()
        {
            if (isModified)
            {
                var result = MessageBox.Show(
                    "Değişiklikler kaydedilsin mi?",
                    "Notepad",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            textBoxContent.Clear();
            currentFilePath = null;
            isModified = false;
            UpdateTitle();
        }

        private void OpenFile()
        {
            if (isModified)
            {
                var result = MessageBox.Show(
                    "Değişiklikler kaydedilsin mi?",
                    "Notepad",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Metin Dosyaları (*.txt)|*.txt|Tüm Dosyalar (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        currentFilePath = openFileDialog.FileName;
                        textBoxContent.Text = File.ReadAllText(currentFilePath);
                        isModified = false;
                        UpdateTitle();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Dosya açılırken hata oluştu:\n{ex.Message}",
                            "Hata",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveFile()
        {
            if (currentFilePath == null)
            {
                SaveFileAs();
            }
            else
            {
                try
                {
                    File.WriteAllText(currentFilePath, textBoxContent.Text);
                    isModified = false;
                    UpdateTitle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Dosya kaydedilirken hata oluştu:\n{ex.Message}",
                        "Hata",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void SaveFileAs()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Metin Dosyaları (*.txt)|*.txt|Tüm Dosyalar (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        currentFilePath = saveFileDialog.FileName;
                        File.WriteAllText(currentFilePath, textBoxContent.Text);
                        isModified = false;
                        UpdateTitle();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Dosya kaydedilirken hata oluştu:\n{ex.Message}",
                            "Hata",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExitApplication()
        {
            if (isModified)
            {
                var result = MessageBox.Show(
                    "Değişiklikler kaydedilsin mi?",
                    "Notepad",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            Application.Exit();
        }

        private void menuItemNew_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void menuItemSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void menuItemSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void textBoxContent_TextChanged(object sender, EventArgs e)
        {
            if (!isModified)
            {
                isModified = true;
                UpdateTitle();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isModified)
            {
                var result = MessageBox.Show(
                    "Değişiklikler kaydedilsin mi?",
                    "Notepad",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    if (isModified) // Hala kaydedilmediyse
                    {
                        e.Cancel = true;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

