   M S B A M L   `   `   `  ����D  @WpfLab2.2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:CompilerWPFCompilerWPF  5   	       CompilerWPF.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:CompilerWPF     5   	     � Title$  Compiler and Interpreter��5   	   $	��600��6*   $	��800��67   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6%   .+�5      �� $��Code Editor��6   $��5,0,5,0q�6*   �� 5      $
��Auto=�6   .��5      �� -   5         ��Name$ CodeTextBox��5      $��5q�6/    �� AcceptsReturn .  �� TextWrapping$
 Wrap=�6O   $	��200��6c   $	��380��6p   5      5      �� 5   
   $��1��6   $��0��6%   .+�5      �� $��Output��6   $��5,0,5,0q�6%   �� 5      $
��Auto=�6   .��5      �� -   5        $ OutputTextBox��5      $��5q�61    �� 
IsReadOnly . $
 Wrap=�6N   $	��200��6b   $	��380��6o   5      5!      �� 5#   
   $��0��6   $��2��6%   $��1��69    �� Orientation$ 
Horizontal=�6F   $��Center=�6_   .+�5$      �� -   5         ��Name$ 
PlayButton��5$      $��5q�6)   $
��10,5q�64   .��5%      �� $ 
Horizontal=�6   .+�5&      �� $��Resources/play.png��6   $��16��68   $��16��6C   $��0,0,5,0q�6O   �� 5'      $
��Play��6    5(      5)      �� 5+      -   5        $ OpenFileButton��5+      $��5q�6-   $
��10,5q�68   .��5,      �� $ 
Horizontal=�6   .+�5-      �� $��Resources/folder.png��6   $��16��6:   $��16��6E   $��0,0,5,0q�6Q   �� 5.      $��	Open File��6    5/      50      �� 51      -   5        $ ClearRegistersButton��51      $��Clear Registers��63   $��5q�6M   $
��10,5q�6X   6g   52      �� 54   
   $��0��6   $��2��6%   $��2��69   .+�55      �� $��Register Values��6   $��5,0,5,0q�6.   �� 56      $
��Auto=�6   .��57      �� -   5        $ RegisterValuesTextBox��57      $��5q�69    . $
 Wrap=�6V   $	��200��6j   $	��780��6w   58      5:      5<      5=      