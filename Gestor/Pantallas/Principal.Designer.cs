
namespace PFG.Gestor
{
	partial class Principal
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.Salir = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.IPGestor = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.ComenzarTerminarJornada = new System.Windows.Forms.Button();
			this.CerrarSesionAdmin = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Roboto Mono", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.ForeColor = System.Drawing.Color.Silver;
			this.label1.Location = new System.Drawing.Point(12, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(239, 35);
			this.label1.TabIndex = 0;
			this.label1.Text = "IP del Gestor:";
			// 
			// Salir
			// 
			this.Salir.BackColor = System.Drawing.Color.DarkRed;
			this.Salir.Font = new System.Drawing.Font("Roboto Mono", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.Salir.ForeColor = System.Drawing.Color.White;
			this.Salir.Location = new System.Drawing.Point(309, 12);
			this.Salir.Name = "Salir";
			this.Salir.Size = new System.Drawing.Size(207, 50);
			this.Salir.TabIndex = 2;
			this.Salir.Text = "Salir";
			this.Salir.UseVisualStyleBackColor = false;
			this.Salir.Click += new System.EventHandler(this.Salir_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.panel1.Controls.Add(this.IPGestor);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(529, 69);
			this.panel1.TabIndex = 4;
			// 
			// IPGestor
			// 
			this.IPGestor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(0)))));
			this.IPGestor.Font = new System.Drawing.Font("Roboto Mono", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.IPGestor.ForeColor = System.Drawing.Color.Black;
			this.IPGestor.Location = new System.Drawing.Point(257, 13);
			this.IPGestor.Margin = new System.Windows.Forms.Padding(0);
			this.IPGestor.Name = "IPGestor";
			this.IPGestor.Size = new System.Drawing.Size(259, 43);
			this.IPGestor.TabIndex = 1;
			this.IPGestor.Text = "___.___.___.___";
			this.IPGestor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.panel3.Controls.Add(this.CerrarSesionAdmin);
			this.panel3.Controls.Add(this.Salir);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 143);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(529, 74);
			this.panel3.TabIndex = 6;
			// 
			// ComenzarTerminarJornada
			// 
			this.ComenzarTerminarJornada.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(0)))));
			this.ComenzarTerminarJornada.Font = new System.Drawing.Font("Roboto Mono", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.ComenzarTerminarJornada.Location = new System.Drawing.Point(12, 81);
			this.ComenzarTerminarJornada.Name = "ComenzarTerminarJornada";
			this.ComenzarTerminarJornada.Size = new System.Drawing.Size(505, 50);
			this.ComenzarTerminarJornada.TabIndex = 3;
			this.ComenzarTerminarJornada.Text = "Comenzar Jornada";
			this.ComenzarTerminarJornada.UseVisualStyleBackColor = false;
			this.ComenzarTerminarJornada.Click += new System.EventHandler(this.ComenzarTerminarJornada_Click);
			// 
			// CerrarSesionAdmin
			// 
			this.CerrarSesionAdmin.BackColor = System.Drawing.Color.MidnightBlue;
			this.CerrarSesionAdmin.Font = new System.Drawing.Font("Roboto Mono", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.CerrarSesionAdmin.ForeColor = System.Drawing.Color.White;
			this.CerrarSesionAdmin.Location = new System.Drawing.Point(12, 12);
			this.CerrarSesionAdmin.Name = "CerrarSesionAdmin";
			this.CerrarSesionAdmin.Size = new System.Drawing.Size(286, 50);
			this.CerrarSesionAdmin.TabIndex = 3;
			this.CerrarSesionAdmin.Text = "Cerrar sesión admin";
			this.CerrarSesionAdmin.UseVisualStyleBackColor = false;
			this.CerrarSesionAdmin.Click += new System.EventHandler(this.CerrarSesionAdmin_Click);
			// 
			// Principal
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Silver;
			this.ClientSize = new System.Drawing.Size(529, 217);
			this.ControlBox = false;
			this.Controls.Add(this.ComenzarTerminarJornada);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Principal";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Gestor";
			this.Load += new System.EventHandler(this.Principal_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button Salir;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button ComenzarTerminarJornada;
		private System.Windows.Forms.Label IPGestor;
		private System.Windows.Forms.Button CerrarSesionAdmin;
	}
}

