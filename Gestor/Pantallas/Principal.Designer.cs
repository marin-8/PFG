
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
			this.IPGestor = new System.Windows.Forms.TextBox();
			this.CerrarYSalir = new System.Windows.Forms.Button();
			this.RegistroIPs = new System.Windows.Forms.ListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.CerrarSesionDesarrollador = new System.Windows.Forms.Button();
			this.RegistroComandos = new System.Windows.Forms.ListBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Roboto Mono", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.ForeColor = System.Drawing.Color.Silver;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(239, 35);
			this.label1.TabIndex = 0;
			this.label1.Text = "IP del Gestor:";
			// 
			// IPGestor
			// 
			this.IPGestor.BackColor = System.Drawing.Color.Silver;
			this.IPGestor.Font = new System.Drawing.Font("Roboto Mono Medium", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.IPGestor.Location = new System.Drawing.Point(257, 12);
			this.IPGestor.Name = "IPGestor";
			this.IPGestor.ReadOnly = true;
			this.IPGestor.Size = new System.Drawing.Size(260, 43);
			this.IPGestor.TabIndex = 1;
			this.IPGestor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// CerrarYSalir
			// 
			this.CerrarYSalir.BackColor = System.Drawing.Color.Silver;
			this.CerrarYSalir.Font = new System.Drawing.Font("Roboto Mono", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.CerrarYSalir.Location = new System.Drawing.Point(12, 12);
			this.CerrarYSalir.Name = "CerrarYSalir";
			this.CerrarYSalir.Size = new System.Drawing.Size(505, 50);
			this.CerrarYSalir.TabIndex = 2;
			this.CerrarYSalir.Text = "Cerrar y Salir";
			this.CerrarYSalir.UseVisualStyleBackColor = false;
			this.CerrarYSalir.Click += new System.EventHandler(this.CerrarYSalir_Click);
			// 
			// RegistroIPs
			// 
			this.RegistroIPs.Font = new System.Drawing.Font("Roboto Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.RegistroIPs.FormattingEnabled = true;
			this.RegistroIPs.ItemHeight = 18;
			this.RegistroIPs.Location = new System.Drawing.Point(12, 52);
			this.RegistroIPs.Name = "RegistroIPs";
			this.RegistroIPs.Size = new System.Drawing.Size(128, 436);
			this.RegistroIPs.TabIndex = 3;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.IPGestor);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(529, 67);
			this.panel1.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Roboto Mono", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label2.Location = new System.Drawing.Point(12, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(129, 28);
			this.label2.TabIndex = 2;
			this.label2.Text = "Registro:";
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.Silver;
			this.panel2.Controls.Add(this.CerrarSesionDesarrollador);
			this.panel2.Controls.Add(this.RegistroComandos);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.RegistroIPs);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 67);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(529, 500);
			this.panel2.TabIndex = 5;
			// 
			// CerrarSesionDesarrollador
			// 
			this.CerrarSesionDesarrollador.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.CerrarSesionDesarrollador.Font = new System.Drawing.Font("Roboto Mono", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.CerrarSesionDesarrollador.ForeColor = System.Drawing.Color.Silver;
			this.CerrarSesionDesarrollador.Location = new System.Drawing.Point(189, 8);
			this.CerrarSesionDesarrollador.Name = "CerrarSesionDesarrollador";
			this.CerrarSesionDesarrollador.Size = new System.Drawing.Size(328, 40);
			this.CerrarSesionDesarrollador.TabIndex = 3;
			this.CerrarSesionDesarrollador.Text = "Cerrar sesión de Desarrollador";
			this.CerrarSesionDesarrollador.UseVisualStyleBackColor = false;
			this.CerrarSesionDesarrollador.Click += new System.EventHandler(this.CerrarSesionDesarrollador_Click);
			// 
			// RegistroComandos
			// 
			this.RegistroComandos.Font = new System.Drawing.Font("Roboto Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.RegistroComandos.FormattingEnabled = true;
			this.RegistroComandos.ItemHeight = 18;
			this.RegistroComandos.Location = new System.Drawing.Point(146, 52);
			this.RegistroComandos.Name = "RegistroComandos";
			this.RegistroComandos.Size = new System.Drawing.Size(371, 436);
			this.RegistroComandos.TabIndex = 4;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.panel3.Controls.Add(this.CerrarYSalir);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 567);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(529, 74);
			this.panel3.TabIndex = 6;
			// 
			// Principal
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(529, 641);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
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
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox IPGestor;
		private System.Windows.Forms.Button CerrarYSalir;
		private System.Windows.Forms.ListBox RegistroIPs;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.ListBox RegistroComandos;
		private System.Windows.Forms.Button CerrarSesionDesarrollador;
	}
}

