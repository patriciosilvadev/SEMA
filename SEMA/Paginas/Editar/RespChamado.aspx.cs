﻿using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace SEMA
{
    public partial class RespChamado : System.Web.UI.Page
    {
        public string mensagem = string.Empty;
        string image;
        public string ImgPath;
        int chamadoID;
        string e_mail;
        DateTime data = DateTime.Now;
        string historico;
        int secretariaID;
        int seq = 0;
        static string prevPage = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            ViewState["Editor"] = temporario.Value;
            descricao.Text = ViewState["Editor"].ToString();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "myKey", "CKEDITOR.replace('MainContent_descricao');", true);

            lblCaminhoImg.Visible = false;
            getStatusColor();
            e_mail = resp_email.Text;

            if (Session["logado"] == null)
            {
                Response.Redirect("../../login.aspx");
            }
            secretariaID = int.Parse(Session["secretaria"].ToString());
            chamadoID = Convert.ToInt32(Request.QueryString["chamadoID"]);
            if (!IsPostBack)
            {
                PreencherCbox();
                GetChamados(chamadoID);
                historicoMsg.Text = getHistorico(chamadoID);
                prevPage = Request.UrlReferrer.ToString();
            }
            if (IsPostBack && img.PostedFile != null)
            {
                if (img.PostedFile.FileName.Length > 0)
                {
                    if (img.PostedFile.ContentLength < 8388608)
                    {
                        try
                        {
                            if (img.HasFile)
                            {
                                try
                                {
                                    //Aqui ele vai filtrar pelo tipo de arquivo
                                    if (img.PostedFile.ContentType == "image/jpeg" ||
                                    img.PostedFile.ContentType == "image/jpg" ||
                                    img.PostedFile.ContentType == "image/png" ||
                                    img.PostedFile.ContentType == "image/gif" ||
                                    img.PostedFile.ContentType == "image/bmp")
                                    {
                                        try
                                        {
                                            //Obtem o  HttpFileCollection
                                            HttpFileCollection hfc = Request.Files;
                                            for (int i = 0; i < hfc.Count; i++)
                                            {
                                                HttpPostedFile hpf = hfc[i];
                                                if (hpf.ContentLength > 0)
                                                {
                                                    //Pega o nome do arquivo
                                                    string nome = Path.GetFileName(hpf.FileName);
                                                    //Pega a extensão do arquivo
                                                    string extensao = Path.GetExtension(hpf.FileName);
                                                    //Gera nome novo do Arquivo numericamente
                                                    string filename = string.Format("{0:00000000000000}", GerarID());
                                                    //Caminho a onde será salvo
                                                    hpf.SaveAs(Server.MapPath("~/dist/img/chamados/") + filename + i
                                                    + extensao);
                                                    //Prefixo p/ img pequena
                                                    var prefixoP = "-80x80";
                                                    //Prefixo p/ img grande
                                                    var prefixoG = "-800x600";
                                                    //pega o arquivo já carregado
                                                    string pth = Server.MapPath("~/dist/img/chamados/")
                                                    + filename + i + extensao;
                                                    //Redefine altura e largura da imagem e Salva o arquivo + prefixo
                                                    ImageResize.resizeImageAndSave(pth, 80, 80, prefixoP);
                                                    ImageResize.resizeImageAndSave(pth, 800, 600, prefixoG);
                                                    image = filename + i + prefixoG + extensao;
                                                }
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            mensagem = "Ocerreu o Seguinte erro: " + ex.Message;
                                            ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
                                        }
                                        // Mensagem se tudo ocorreu bem
                                        //imgSel.ImageUrl = "dist/img/chamados/" + image;
                                        ImgPath = "/dist/img/chamados/" + image;
                                        Image1.ImageUrl = ImgPath;
                                        imgSel.ImageUrl = ImgPath;
                                        lblCaminhoImg.Text = image;
                                        mensagem = "Upload da Imagem feito com Sucesso!!!";
                                        ClientScript.RegisterStartupScript(GetType(), "Popup", "sucesso();", true);
                                    }
                                    else
                                    {
                                        // Mensagem notifica que é permitido carregar apenas 
                                        // as imagens definida la em cima.
                                        mensagem = "É permitido carregar apenas imagens!";
                                        ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    // Mensagem notifica quando ocorre erros
                                    mensagem = "O arquivo não pôde ser carregado. O seguinte erro ocorreu: " + ex.Message;
                                    ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            // Mensagem notifica quando ocorre erros
                            mensagem = "O arquivo não pôde ser carregado. O seguinte erro ocorreu: " + ex.Message;
                            ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
                        }
                    }
                    else
                    {
                        // Mensagem notifica quando imagem é superior a 8 MB
                        mensagem = "Não é permitido carregar mais do que 8 MB";
                        ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
                    }
                }
            }
            else
            if (lblCaminhoImg.Text == "user-800x600.png")
            {
                ImgPath = "/dist/img/chamados/user-800x600.png";
                Image1.ImageUrl = ImgPath;
                imgSel.ImageUrl = ImgPath;
                lblCaminhoImg.Text = "user-800x600.png";
            }
        }
        private void PreencherCbox()
        {
            semaEntities ctx = new semaEntities();
            var resultado = (from t in ctx.usuarios
                             where t.secretariaID == secretariaID
                             select new
                             {
                                 t.id,
                                 t.nome,
                             });
            cboxUsuario.Items.Insert(0, new ListItem("Selecione", "Selecione"));
            foreach (var item in resultado)
            {
                string valor = Convert.ToString(item.id);
                cboxUsuario.Items.Add(new ListItem(item.nome, valor));
            }
        }
        public Int64 GerarID()
        {
            try
            {
                DateTime data = new DateTime();
                data = DateTime.Now;
                string s = data.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                return Convert.ToInt64(s);
            }
            catch (System.Exception ex)
            {
                mensagem = "Ocorreu o Seguinte erro: " + ex.Message;
                ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
                throw;
            }
        }
        private void getStatusColor()
        {
            for (int i = 0; i < cboxStatus.Items.Count; i++)
            {
                if (cboxStatus.SelectedValue == "Finalizado")
                {
                    string verde = "#478978";
                    cboxStatus.ForeColor = System.Drawing.Color.White;
                    cboxStatus.BackColor = System.Drawing.ColorTranslator.FromHtml(verde);
                }
                if (cboxStatus.SelectedValue == "Aberto")
                {
                    string azul = "#3C8DBC";
                    cboxStatus.ForeColor = System.Drawing.Color.White;
                    cboxStatus.BackColor = System.Drawing.ColorTranslator.FromHtml(azul);
                }
                if (cboxStatus.SelectedValue == "Em Atendimento")
                {
                    string laranja = "#F39C12";
                    cboxStatus.ForeColor = System.Drawing.Color.White;
                    cboxStatus.BackColor = System.Drawing.ColorTranslator.FromHtml(laranja);
                }
                if (cboxStatus.SelectedValue == "Pendente")
                {
                    string vermelho = "#DD4B39";
                    cboxStatus.ForeColor = System.Drawing.Color.White;
                    cboxStatus.BackColor = System.Drawing.ColorTranslator.FromHtml(vermelho);
                }
            }
        }
        public void GetChamados(int cod)
        {
            semaEntities ctx = new semaEntities();
            var resultado = (from a in ctx.chamadoes
                             join b in ctx.assuntoes on a.assunto equals b.id
                             join c in ctx.topicos on a.topico equals c.id
                             where a.id == cod
                             select new
                             {
                                 a.id,
                                 a.protocolo,
                                 a.nome,
                                 a.cpf,
                                 a.email,
                                 a.telefone,
                                 assunto = b.descricao,
                                 topico = c.descricao,
                                 a.status,
                                 a.img,
                                 a.usuario_responsavel,
                                 a.anonimo,
                                 a.bairro,
                                 a.cep,
                                 a.cidade,
                                 a.data,
                                 a.numero,
                                 a.rua,
                                 a.user_cadastrou,
                                 a.envia_whatsapp
                             });
            foreach (var item in resultado)
            {

                resp_txtProtocolo.Text = item.protocolo;
                resp_nome.Text = item.nome;
                resp_email.Text = item.email;
                resp_cpf.Text = item.cpf;
                resp_telefone.Text = item.telefone;
                resp_cboxAssunto.Items.Add(new ListItem(item.assunto, item.assunto));
                resp_cboxTopico.Items.Add(new ListItem(item.topico, item.topico));
                resp_cboxStatus.Items.Add(new ListItem(item.status, item.status));
                cboxUsuario.SelectedValue = Convert.ToString(item.usuario_responsavel);
                ImgPath = "/dist/img/chamados/" + item.img;
                lblCaminhoImg.Text = item.img;
                resp_txtCEP.Text = item.cep;
                resp_txtRua.Text = item.rua;
                resp_txtNumero.Text = item.numero;
                resp_txtBairro.Text = item.bairro;
                resp_txtCidade.Text = item.cidade;
                if (item.anonimo == "SIM")
                {
                    resp_checkDenuncia.Checked = true;
                }
                else
                {
                    resp_checkDenuncia.Checked = false;
                }
                if (item.envia_whatsapp == "SIM")
                {
                    resp_checkWhatsapp.Checked = true;
                }
                else
                {
                    resp_checkWhatsapp.Checked = false;
                }
            }
        }
        private string getHistorico(int cod)
        {
            StringBuilder sb = new StringBuilder();
            semaEntities ctx = new semaEntities();
            var resultado = (from a in ctx.historicoes
                             where a.chamadoID == cod
                             orderby a.sequencia ascending
                             select new
                             {
                                 a.data,
                                 a.mensagem,
                                 a.sequencia,
                                 a.origem,
                             });
            foreach (var item in resultado)
            {
                if (item.origem == "cidadao")
                {
                    sb.Append("<div class='container1'><img src ='/dist/img/cidadao.jpg' alt='Avatar'>");
                    sb.Append(item.mensagem);
                    
                    sb.Append("<span class='time-right' style='text-align:justify;display:block;'>" + item.data + "</span></div>");
                }
                if (item.origem == "agente")
                {
                    sb.Append("<div class='container1 darker'><img src ='/dist/img/sema.jpg' alt='Avatar' class='right'>");
                    sb.Append(item.mensagem);
                    sb.Append("<span class='time-left'>" + item.data + "</span></div>");
                }
                historico = historico + item.mensagem;
            }
            return sb.ToString();
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (cboxUsuario.SelectedIndex == 0)
            {
                mensagem = "Deve Selecionar o usuario Responsavel pelo atendimento do chamado";
                ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
                cboxUsuario.Focus();
            }
            else
            if (cboxStatus.SelectedIndex == 0)
            {
                mensagem = "Favor atualizar o Status do Chamado";
                ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
                cboxStatus.Focus();
            }
            else
            if (descricao.Text == "")
            {
                mensagem = "Favor preencher a descrição da resposta do chamado";
                ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
                descricao.Focus();
            }
            else
            if (descricao.Text.Length < 200)
            {
                mensagem = "A descrição do chamado esta muito curta, deve conter no mínimo 200 caracteres !";
                ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
                descricao.Focus();
            }
            else
            {
                try
                {
                    semaEntities ctx = new semaEntities();
                    chamado cha = ctx.chamadoes.First(p => p.id == chamadoID);
                    cha.usuario_responsavel = int.Parse(cboxUsuario.SelectedValue);
                    cha.status = cboxStatus.SelectedValue;
                    //cha.resposta = descricao.Text;
                    cha.img = lblCaminhoImg.Text;
                    ctx.SaveChanges();
                    pushMensage();
                    gravaHistorico();
                    mensagem = "Gravado com Sucesso!";
                    if (resp_email.Text != "")
                    {
                        Email();
                    }
                    ClientScript.RegisterStartupScript(GetType(), "Popup", "sucesso();", true);
                }
                catch (System.Exception ex)
                {
                    mensagem = "Ocorreu o Seguinte erro ao tentar gravar " + ex.Message;
                    ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
                }
            }
        }
        // pega o valor do sequencia da ultima mensagem na tabela historico
        private void pushMensage()
        {
            try
            {
                semaEntities ctx = new semaEntities();
                var resultado = (from t in ctx.historicoes
                                 where t.chamadoID == chamadoID
                                 orderby t.sequencia descending
                                 select new
                                 {
                                     t.sequencia,
                                     t.mensagem,
                                 }).Take(1);
                foreach (var item in resultado)
                {
                    seq = item.sequencia.Value;
                }
            }
            catch (System.Exception ex)
            {
                mensagem = "Ocorreu o seguinte erro ao tentar consultar ultima mensagem: " + ex.Message;
                ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
            }
        }
        // grava a mensagem na tabela historico
        private void gravaHistorico()
        {
            try
            {
                seq = seq + 1;
                semaEntities ctx = new semaEntities();
                historico his = new historico();
                his.chamadoID = chamadoID;
                his.mensagem = "<p>Enviada em: " + data + "</p></br>" + descricao.Text;
                his.sequencia = seq;
                his.origem = "agente";
                his.data = data;
                ctx.historicoes.Add(his);
                ctx.SaveChanges();
            }
            catch (System.Exception ex)
            {
                mensagem = "Ocorreu o seguinte erro ao gravar mensagem: " + ex.Message;
                ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
            }
        }
        private void Email()
        {
            try
            {
                int sec = int.Parse(Session["secretaria"].ToString());
                semaEntities ctx = new semaEntities();
                configuraco cfg = ctx.configuracoes.First(p => p.secretariaID == sec);
                string protocolo = resp_txtProtocolo.Text.Trim();
                string nome = resp_nome.Text.Trim();
                string telefone = resp_telefone.Text.Trim();
                string email = resp_email.Text.Trim();
                string cpf = resp_cpf.Text.Trim();
                string assunto = resp_cboxAssunto.SelectedItem.ToString();
                string topico = resp_cboxTopico.SelectedItem.ToString();
                string status = resp_cboxStatus.SelectedItem.ToString();
                string body = cfg.bodyEmailResposta.ToString();
                if (body.Contains("[nome]"))
                {
                    body = body.Replace("[nome]", resp_nome.Text);
                }
                if (body.Contains("[protocolo]"))
                {
                    body = body.Replace("[protocolo]", resp_txtProtocolo.Text);
                }
                if (body.Contains("[telefone]"))
                {
                    body = body.Replace("[telefone]", resp_telefone.Text);
                }
                if (body.Contains("[email]"))
                {
                    body = body.Replace("[email]", resp_email.Text);
                }
                if (body.Contains("[cpf]"))
                {
                    body = body.Replace("[cpf]", resp_cpf.Text);
                }
                if (body.Contains("[assunto]"))
                {
                    body = body.Replace("[assunto]", resp_cboxAssunto.SelectedItem.ToString());
                }
                if (body.Contains("[topico]"))
                {
                    body = body.Replace("[topico]", resp_cboxTopico.SelectedItem.ToString());
                }
                if (body.Contains("[status]"))
                {
                    body = body.Replace("[status]", resp_cboxStatus.SelectedItem.ToString());
                }
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(e_mail, cfg.nomeRemetente);
                mailMessage.To.Add(e_mail.ToLower());
                mailMessage.Subject = cfg.assunto;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = body;
                mailMessage.Priority = MailPriority.High;
                SmtpClient smtpClient = new SmtpClient(cfg.smtp, int.Parse(cfg.porta));
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(cfg.email, cfg.senhaEmail);
                smtpClient.Send(mailMessage);
                ClientScript.RegisterStartupScript(GetType(), "Popup", "sucesso();", true);
            }
            catch (System.Exception ex)
            {
                mensagem = "Erro ao enviar e-mail: " + ex.Message;
                ClientScript.RegisterStartupScript(GetType(), "Popup", "erro();", true);
            }
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect(prevPage);
        }
        protected void cboxStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            getStatusColor();
        }
        protected void btnVoltar_resp_Click(object sender, EventArgs e)
        {
            Response.Redirect(prevPage);
        }
        protected void Image1_Click(object sender, ImageClickEventArgs e)
        {
            ModalPlaceHolder.Visible = true;
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "UpdatePanel1StartupScript", "setTimeout('window.scrollTo(0,0)', 0);", true);
        }
        protected void btnModalCloseHeader_Click(object sender, EventArgs e)
        {
            ModalPlaceHolder.Visible = false;
        }
        //protected string UploadFolderPath = "~/dist/img/chamados/";
        protected void FileUploadComplete(object sender, EventArgs e)
        {
            // string filename = System.IO.Path.GetFileName(img.FileName);
            // img.SaveAs(Server.MapPath(this.UploadFolderPath) + filename);
            //lblCaminhoImg.Text = image;
        }
    }
}