   M S B A M L   `   `   `  ����
  Server    Server.GroupInfoUC   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -   5          >�Name$  UserControl��5       � 	Resources 5         WrappedColumnHeaderStyle �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property  dgHeaderBrush �� Value ��Name �� Triggers �� Value �� Binding	 �� Path
 �� RelativeSource �� AncestorType �� 
TargetName*  $System.Windows.Controls.DataGridCell �� 
StartPoint �� EndPoint � Color � Offset  BrownGradientFixed � MappingMode  GreyGradientFixed  DataGridNoSize&   System.Windows.Controls.DataGrid  DataGridDocked��%   &	        '  �    &	 C    &	 �    &	 q    '��     &	 �    &	 b    �� 5   
   "  61   .�5      ��  � ��# �  6+   �� 5       � ��$ Center=�62   �� 5       � ��$ Segoe UI)�6+   �� 5       � ��$ 18(�6)   �� 5       � �� 5      �� 5      .R�5      �� $ 	TextBlock��6$   $��NoWrap=�67   ��6M   ��  5      �� 5      $ Center��6�    6*   �� $ 	 HorizontalContentAlignment�
 �� *FindAncestor+"  .a�5   "   �� $ 	TextBlock��6)    �@��TextAlignment$ Center=�6Y   5      �� 5      $
 Left��6�    6*   �� $ 	 HorizontalContentAlignment�
 �� *FindAncestor+"  .a�5   "   �� $ 	TextBlock��6)    �@��TextAlignment$
 Left=�6Y   5      �� 5      $ Right��6�    6*   �� $ 	 HorizontalContentAlignment�
 �� *FindAncestor+"  .a�5   "   �� $ 	TextBlock��6)    �@��TextAlignment$ Right=�6Y   5      5       5"      5#      �� 5%   
   "  6   .�5&      ��  �@��TextAlignment$ Center=�64   �� 5'       � ��$ Segoe UI)�6+   �� 5(       � ��$ 16(�6)   5)      �� 5+      $	 0,0&�6.   $	 0,1&�6?   .��5,      � $
 #fff��6   $ 0Q�6(   � 5-      $
 #999��6   $ 1Q�6(   5.      �� 50   
   $ Absolute=�69   $	 0,0&�6P   $
 0,48&�6a   .��51      � $ 	#FF39291F��6   � 52      $ 	#FF6E6056��6   $ 1Q�6-   53      �� 55   
   $ Absolute=�68   $	 0,0&�6O   $
 0,90&�6`   .��56      � $ 	#FF252525��6   � 57      $ 	#FFD0D0D0��6   $ 1Q�6-   58      �� 5:   
   " ��6   .�5;      ��  � ��$ 20(�6)   �� 5<       � ��$ Regular"�6+   �� 5=       � �� 6,   Z� �� 5>       � ��
 �����6+   �� 5?       � �� 6+   Z� 5@      �� 5B   
   "  6'   .�5C      ��  � ��$	 Top=�62   �� 5D       � ��$ 5,5,5,0q�6'   5E      �� 5G   
   "  6'   .�5H      ��  � ��$ 5,5,5,15q�6'   5I      5J      .��5L      � -   5         �Name$ 
LayoutRoot��5L      #��[ 6    � RowDefinitions 5N   
   �� 5O      $��45 �6   �� 5P      .+�5S   
   �� -   5         ��Name$ lblTitle��5S      $��Group Information��6"   $��0q�6>   $��Center=�6I   $��Center=�6k     5U   
   -   5          Name$ dgGroupInfo��5U      $��1��6(      SelectionMode$ Single=�65      
IsReadOnly .    CanUserReorderColumns .     CanUserResizeColumns .     CanUserSortColumns .     CanUserResizeRows .     AlternatingRowBackground �	#FFE2E2E2   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5W         RowHeaderWidth$ 0Q�6      AutoGenerateColumns .      ColumnHeaderStyle#  [  6C   #��[ 5X      5[      5\      