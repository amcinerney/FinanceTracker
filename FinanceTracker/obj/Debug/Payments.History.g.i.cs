﻿#pragma checksum "..\..\Payments.History.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "011153F85C173C1B8971A8A492829EA8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace FinanceTracker {
    
    
    /// <summary>
    /// PaymentsHistoryForm
    /// </summary>
    public partial class PaymentsHistoryForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\Payments.History.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FinanceTracker.PaymentsHistoryForm paymentHistoryMainWindow;
        
        #line default
        #line hidden
        
        
        #line 6 "..\..\Payments.History.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid paymentHistoryGrid;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\Payments.History.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button phCloseButton;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\Payments.History.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox phYearCB;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\Payments.History.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox phMonthCB;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\Payments.History.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox phCreditorCB;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\Payments.History.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label phTotalLabel;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\Payments.History.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label phAverageLabel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FinanceTracker;component/payments.history.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Payments.History.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.paymentHistoryMainWindow = ((FinanceTracker.PaymentsHistoryForm)(target));
            return;
            case 2:
            this.paymentHistoryGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.phCloseButton = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\Payments.History.xaml"
            this.phCloseButton.Click += new System.Windows.RoutedEventHandler(this.PHCloseButtonClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.phYearCB = ((System.Windows.Controls.ComboBox)(target));
            
            #line 20 "..\..\Payments.History.xaml"
            this.phYearCB.DropDownClosed += new System.EventHandler(this.OptionSelected);
            
            #line default
            #line hidden
            return;
            case 5:
            this.phMonthCB = ((System.Windows.Controls.ComboBox)(target));
            
            #line 22 "..\..\Payments.History.xaml"
            this.phMonthCB.DropDownClosed += new System.EventHandler(this.OptionSelected);
            
            #line default
            #line hidden
            return;
            case 6:
            this.phCreditorCB = ((System.Windows.Controls.ComboBox)(target));
            
            #line 38 "..\..\Payments.History.xaml"
            this.phCreditorCB.DropDownClosed += new System.EventHandler(this.OptionSelected);
            
            #line default
            #line hidden
            return;
            case 7:
            this.phTotalLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.phAverageLabel = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

