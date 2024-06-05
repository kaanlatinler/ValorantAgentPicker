using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
using System.Runtime.InteropServices;
using ValorantAgentPicker.Models;

namespace ValorantAgentPicker
{
    public partial class MainForm : Form
    {
        private readonly Database _database;

        public MainForm(Database database)
        {
            InitializeComponent();
            _database = database;

            this.KeyDown += MainForm_KeyDown;
            this.KeyPreview = true;
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]

        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        int mouseX, mouseY;
        int firstX, firstY, lastX, lastY;
        int time = 0, selectedTime = 30;
        bool timer1 = false, timer2 = true, timer3 = false, suppressEvents = false;

        private void MainForm_Load(object sender, EventArgs e)
        {
            FillComboBox();
            GetProfiles();
            time2.Checked = timer2;
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                timerForPick.Stop();
                time = 0;
            }
        }

        private void NewAgentCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerForNewAgent.Start();

            string selected = NewAgentCB.SelectedItem.ToString();

            var agent = _database.GetAgentByName(selected);

            NewAgentPic.ImageLocation = agent.AgentPhoto;
        }

        private void AddNewAgentBTN_Click(object sender, EventArgs e)
        {
            timerForNewAgent.Stop();
            X.Text = "";
            Y.Text = "";

            string selected = NewAgentCB.SelectedItem.ToString();

            var agent = _database.GetAgentByName(selected);

            firstX = Convert.ToInt32(FirstMx.Text);
            lastX = Convert.ToInt32(LastMx.Text);
            firstY = Convert.ToInt32(FirstMy.Text);
            lastY = Convert.ToInt32(LastMy.Text);

            var profile = new Profile
            {
                Agent = agent,
                AgentId = agent.AgentId,
                FirstMx = firstX,
                LastMx = lastX,
                FirstMy = firstY,
                LastMy = lastY,
            };

            bool saved = _database.SaveNewProfile(profile);

            if (saved)
            {
                SavedMsg.Icon = MessageDialogIcon.Information;
                SavedMsg.Caption = "Kayýt Baþarýlý!";
                SavedMsg.Show();
            }
            else
            {
                SavedMsg.Icon = MessageDialogIcon.Error;
                SavedMsg.Caption = "Kayýt Sýrasýnda Bir Hata Meydana Geldi Tekrar Deneyiniz!!";
                SavedMsg.Show();
            }

            GetProfiles();
        }

        private void UpdateBTN_Click(object sender, EventArgs e)
        {
            timerForNewAgent.Stop();

            int profileID = Convert.ToInt32(ProfileIDLBL.Text);
            var profile = _database.GetProfileByID(profileID);

            profile.FirstMx = Convert.ToInt32(AgentFirstX.Text);
            profile.FirstMy = Convert.ToInt32(AgentFirstY.Text);
            profile.LastMx = Convert.ToInt32(AgentLastX.Text);
            profile.LastMy = Convert.ToInt32(AgentLastY.Text);

            bool saved = _database.UpdateProfile(profile);

            if (saved)
            {
                SavedMsg.Icon = MessageDialogIcon.Information;
                SavedMsg.Caption = "Kayýt Baþarýlý!";
                SavedMsg.Show();
            }
            else
            {
                SavedMsg.Icon = MessageDialogIcon.Error;
                SavedMsg.Caption = "Kayýt Sýrasýnda Bir Hata Meydana Geldi Tekrar Deneyiniz!!";
                SavedMsg.Show();
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            int profileID = Convert.ToInt32(ProfileIDLBL.Text);
            bool profile = _database.DeleteProfile(profileID);

            if (profile)
            {
                ProfileIDLBL.Text = "";
                AgentNameLBL.Text = "";
                AgentFirstX.Text = "";
                AgentFirstY.Text = "";
                AgentLastX.Text = "";
                AgentLastY.Text = "";

                SavedMsg.Icon = MessageDialogIcon.Information;
                SavedMsg.Caption = "Silme Baþarýlý!";
                SavedMsg.Show();
            }
            else
            {
                SavedMsg.Icon = MessageDialogIcon.Error;
                SavedMsg.Caption = "Silme Sýrasýnda Bir Hata Meydana Geldi Tekrar Deneyiniz!!";
                SavedMsg.Show();
            }

            GetProfiles();
        }

        private void InstaLockBTN_Click(object sender, EventArgs e)
        {
            timerForPick.Start();
        }

        private void timerForPick_Tick(object sender, EventArgs e)
        {
            firstX = Convert.ToInt32(AgentFirstX.Text);
            firstY = Convert.ToInt32(AgentFirstY.Text);
            lastX = Convert.ToInt32(AgentLastX.Text);
            lastY = Convert.ToInt32(AgentLastY.Text);

            if (time < selectedTime * 10)
            {
                Cursor.Position = new Point(firstX, firstY);
                Thread.Sleep(25);
                Lock(new Point(MousePosition.X, MousePosition.Y));
                Thread.Sleep(25);
                Cursor.Position = new Point(lastX, lastY);
                Thread.Sleep(25);
                Lock(new Point(MousePosition.X, MousePosition.Y));
                Thread.Sleep(25);
                time++;
            }
            else
            {
                firstX = 0;
                firstY = 0;
                lastX = 0;
                lastY = 0;
                time = 0;
                timerForPick.Stop();
            }
        }

        private void timerForNewAgent_Tick(object sender, EventArgs e)
        {
            MouseTracker();
        }

        private void time1_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEvents) return;

            suppressEvents = true;
            timer1 = time1.Checked;
            timer2 = false;
            timer3 = false;

            selectedTime = time1.Checked ? 10 : 0;

            time2.Checked = timer2;
            time3.Checked = timer3;
            suppressEvents = false;
        }

        private void time2_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEvents) return;

            suppressEvents = true;
            timer1 = false;
            timer2 = time2.Checked;
            timer3 = false;

            selectedTime = time2.Checked ? 30 : 0;

            time1.Checked = timer1;
            time3.Checked = timer3;
            suppressEvents = false;
        }

        private void time3_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressEvents) return;

            suppressEvents = true;
            timer1 = false;
            timer2 = false;
            timer3 = time3.Checked;

            selectedTime = time3.Checked ? 60 : 0;

            time2.Checked = timer2;
            time1.Checked = timer1;
            suppressEvents = false;
        }

        //Utils

        private enum MouseEventFlags
        {
            LeftDown = 2,
            LeftUp = 4,
        }

        private void Lock(Point point)
        {
            mouse_event((int)(MouseEventFlags.LeftDown), point.X, point.Y, 0, 0);
            mouse_event((int)(MouseEventFlags.LeftUp), point.X, point.Y, 0, 0);
        }

        private void FillComboBox()
        {
            var agents = _database.GetAgents();
            NewAgentCB.Items.Clear();
            foreach (var item in agents)
            {
                NewAgentCB.Items.Add(item.AgentName);
            }
        }

        private void MouseTracker()
        {
            mouseX = MousePosition.X;
            mouseY = MousePosition.Y;

            X.Text = mouseX.ToString();
            Y.Text = mouseY.ToString();
        }

        private void GetProfiles()
        {
            AgentsPanel.Controls.Clear();
            AgentsPanel.AutoScroll = true;

            int x = 0, y = 0;

            var profiles = _database.GetProfiles();

            foreach (var item in profiles)
            {
                string imgPath = item.Agent.AgentPhoto;
                Image img = Image.FromFile(imgPath);
                Guna2ImageButton imgBtn = new Guna2ImageButton();
                imgBtn.Location = new Point(x, y);
                imgBtn.Image = img;
                imgBtn.ImageSize = new Size(90, 139);
                imgBtn.Size = new Size(100, 180);
                imgBtn.Cursor = Cursors.Hand;
                imgBtn.Tag = item.ProfileId;
                imgBtn.AnimatedGIF = false;
                imgBtn.CheckedState.Parent = null;
                imgBtn.ImageRotate = 0F;
                imgBtn.TabIndex = 0;
                imgBtn.TabStop = false;
                imgBtn.HoverState.ImageSize = new Size(120, 184);

                imgBtn.MouseLeave += ImgBtn_MouseLeave;
                imgBtn.Click += ImgBtn_Click;


                x += 150;

                if (x > 1050)
                {
                    x = 150;
                    y += 100;
                }
                AgentsPanel.Controls.Add(imgBtn);
            }
        }

        private void ImgBtn_Click(object? sender, EventArgs e)
        {
            timerForNewAgent.Start();

            var btn = sender as Guna2ImageButton;
            var profileID = (int)btn.Tag;
            var profile = _database.GetProfileByID(profileID);

            ProfileIDLBL.Text = profile.ProfileId.ToString();
            AgentNameLBL.Text = profile.Agent.AgentName;
            AgentFirstX.Text = profile.FirstMx.ToString();
            AgentFirstY.Text = profile.FirstMy.ToString();
            AgentLastX.Text = profile.LastMx.ToString();
            AgentLastY.Text = profile.LastMy.ToString();
        }

        private void ImgBtn_MouseLeave(object? sender, EventArgs e)
        {
            var btn = sender as Guna2ImageButton;
            btn.ImageSize = new Size(90, 139);
        }
    }
}
