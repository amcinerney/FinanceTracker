   M S B A M L   `   `   `  ����  FinanceTracker(    "FinanceTracker.PaymentsHistoryForm   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          �Name$  paymentHistoryMainWindow��5   	    � Title$ Payment History��5   	   $	��500��6!   $	��900��6.   .��5      � ���	#FFE5E5E5.+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ paymentHistoryGrid��5      $
��Left=�6/   $��	75,57,0,0q�6J   $	��Top=�6]   $	��312��6u   $	��745��6�    � 	Resources 5       �� 
TargetType   RightAligElementStyle �� Property �� Value��%c   &	        �� 5      " ��6   .�5	      ��  �@��TextAlignment$ Right=�66   5
      5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Creditor��6B   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6%   �� *
creditor+6B     5      $ Amount Paid��6@   
   ElementStyle#
 [  6V   	 6%   �� *amount+6V     5      $ 	Date Paid��6>   #
 [  6R   	 6%   �� *date+6R   5      �� 5   
   -   5         ��Name$ phCloseButton��5      $��Close��6(   $
��Left=�68   $��425,410,0,0q�6S   $	��Top=�6h   $��75��6�    J� RenderTransformOrigin$ -0.133,0.55&�6�   6�   �� 5   
   $��Year :��6   $
��Left=�6!   $��	75,26,0,0q�6<   $	��Top=�6O   �� 5   
   -   5         ��Name$ phYearCB��5      $
��Left=�6%   $��
119,30,0,0q�6@   $	��Top=�6T   $	��120��6l   $ 0.208,-0.909&�6x   6�   �� 5   
   $��Month :��6   $
��Left=�6"   $��
244,26,0,0q�6=   $	��Top=�6Q   $ -0.237,0.154&�6i   �� 5   
   -   5        $ 	phMonthCB��5      $
��Left=�6&   $��
300,30,0,0q�6A   $	��Top=�6U   $	��120��6m   .@�5      �� $	��All��6   �� 5      $��January��6   �� 5      $��February��6   �� 5      $��March��6   �� 5      $��April��6   �� 5      $	��May��6   �� 5      $
��June��6   �� 5      $
��July��6   �� 5      $��August��6   �� 5       $��	September��6   �� 5!      $��October��6   �� 5"      $��November��6   �� 5#      $��December��6   5$      �� 5%   
   $��
Creditor :��6   $
��Left=�6%   $��
425,26,0,0q�6@   $	��Top=�6T   �� 5&   
   -   5        $ phCreditorCB��5&      $
��Left=�6)   $��
489,30,0,0q�6D   $	��Top=�6X   $	��120��6p   6|   �� 5'   
   $��Total :��6   $
��Left=�6"   $��
75,374,0,0q�6=   $	��Top=�6Q   �� 5(   
   -   5         ��Name$ phTotalLabel��5(      $�� ��6&   $
��Left=�61   $��117,374,0,0q�6L   $	��Top=�6a   �� 5)   
   $��	Average :��6   $
��Left=�6$   $��227,374,0,0q�6?   $	��Top=�6T   $ 1.974,0.885&�6l   �� 5*   
   -   5        $ phAverageLabel��5*      $�� ��6(   $
��Left=�63   $��286,374,0,0q�6N   $	��Top=�6c   5,      5-      