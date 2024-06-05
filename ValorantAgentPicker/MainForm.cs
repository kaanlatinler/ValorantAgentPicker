using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
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
        }

        int mouseX, mouseY;
        int firstX, firstY, lastX, lastY;

        private void MainForm_Load(object sender, EventArgs e)
        {
            FillComboBox();
            GetProfiles();
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
        }

        private void timerForNewAgent_Tick(object sender, EventArgs e)
        {
            MouseTracker();
        }

        //Utils

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

        private void UpdateBTN_Click(object sender, EventArgs e)
        {
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
    }
}
