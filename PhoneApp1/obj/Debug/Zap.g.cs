﻿#pragma checksum "C:\Users\lavin\Source\Repos\Wpp\PhoneApp1\Zap.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4C2CA7BD0060F371C8ABC80A879BE9B2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace PhoneApp1 {
    
    
    public partial class Zap : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Pivot pivotZap;
        
        internal System.Windows.Controls.ListBox ListContatos;
        
        internal System.Windows.Controls.ListBox ListGrupos;
        
        internal System.Windows.Controls.ListBox ListDetalhesGrupo;
        
        internal System.Windows.Controls.TextBlock tbNomeGP;
        
        internal System.Windows.Controls.ListBox ListUsersSend;
        
        internal System.Windows.Controls.TextBox txtMsg;
        
        internal System.Windows.Controls.Button btnEnviarMsg;
        
        internal System.Windows.Controls.ListBox ListGPSend;
        
        internal System.Windows.Controls.TextBox txtMsgGP;
        
        internal System.Windows.Controls.Button btnEnviarMsgGP;
        
        internal System.Windows.Controls.TextBox txtNome;
        
        internal System.Windows.Controls.Button btnInserirContato;
        
        internal System.Windows.Controls.TextBlock textBlockFT;
        
        internal System.Windows.Controls.Image imgSelectedImage;
        
        internal System.Windows.Controls.Button btnPicture;
        
        internal System.Windows.Controls.TextBox txtDescricao;
        
        internal System.Windows.Controls.ListBox ListSelectUsers;
        
        internal System.Windows.Controls.Button btnInserirGrupo;
        
        internal System.Windows.Controls.ListBox ListSelectUsersEdit;
        
        internal System.Windows.Controls.TextBox txtNomeEdit;
        
        internal System.Windows.Controls.TextBox txtFotoEdit;
        
        internal System.Windows.Controls.Button btnEditContato;
        
        internal System.Windows.Controls.Button btnDelContato;
        
        internal Microsoft.Phone.Controls.LongListSelector ListSelectGPEdit;
        
        internal System.Windows.Controls.TextBox txtDescEdit;
        
        internal System.Windows.Controls.Button btnEditGP;
        
        internal System.Windows.Controls.Button btnDelGP;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/PhoneApp1;component/Zap.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.pivotZap = ((Microsoft.Phone.Controls.Pivot)(this.FindName("pivotZap")));
            this.ListContatos = ((System.Windows.Controls.ListBox)(this.FindName("ListContatos")));
            this.ListGrupos = ((System.Windows.Controls.ListBox)(this.FindName("ListGrupos")));
            this.ListDetalhesGrupo = ((System.Windows.Controls.ListBox)(this.FindName("ListDetalhesGrupo")));
            this.tbNomeGP = ((System.Windows.Controls.TextBlock)(this.FindName("tbNomeGP")));
            this.ListUsersSend = ((System.Windows.Controls.ListBox)(this.FindName("ListUsersSend")));
            this.txtMsg = ((System.Windows.Controls.TextBox)(this.FindName("txtMsg")));
            this.btnEnviarMsg = ((System.Windows.Controls.Button)(this.FindName("btnEnviarMsg")));
            this.ListGPSend = ((System.Windows.Controls.ListBox)(this.FindName("ListGPSend")));
            this.txtMsgGP = ((System.Windows.Controls.TextBox)(this.FindName("txtMsgGP")));
            this.btnEnviarMsgGP = ((System.Windows.Controls.Button)(this.FindName("btnEnviarMsgGP")));
            this.txtNome = ((System.Windows.Controls.TextBox)(this.FindName("txtNome")));
            this.btnInserirContato = ((System.Windows.Controls.Button)(this.FindName("btnInserirContato")));
            this.textBlockFT = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockFT")));
            this.imgSelectedImage = ((System.Windows.Controls.Image)(this.FindName("imgSelectedImage")));
            this.btnPicture = ((System.Windows.Controls.Button)(this.FindName("btnPicture")));
            this.txtDescricao = ((System.Windows.Controls.TextBox)(this.FindName("txtDescricao")));
            this.ListSelectUsers = ((System.Windows.Controls.ListBox)(this.FindName("ListSelectUsers")));
            this.btnInserirGrupo = ((System.Windows.Controls.Button)(this.FindName("btnInserirGrupo")));
            this.ListSelectUsersEdit = ((System.Windows.Controls.ListBox)(this.FindName("ListSelectUsersEdit")));
            this.txtNomeEdit = ((System.Windows.Controls.TextBox)(this.FindName("txtNomeEdit")));
            this.txtFotoEdit = ((System.Windows.Controls.TextBox)(this.FindName("txtFotoEdit")));
            this.btnEditContato = ((System.Windows.Controls.Button)(this.FindName("btnEditContato")));
            this.btnDelContato = ((System.Windows.Controls.Button)(this.FindName("btnDelContato")));
            this.ListSelectGPEdit = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("ListSelectGPEdit")));
            this.txtDescEdit = ((System.Windows.Controls.TextBox)(this.FindName("txtDescEdit")));
            this.btnEditGP = ((System.Windows.Controls.Button)(this.FindName("btnEditGP")));
            this.btnDelGP = ((System.Windows.Controls.Button)(this.FindName("btnDelGP")));
        }
    }
}

