   M S B A M L   `   `   `  ????E  ATruitjesUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TruitjesUI
TruitjesUI  5   	   /    )TruitjesUI.WindowBestellingSelecteerKlant   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:TruitjesUI     5   	     ? Title$$  WindowBestellingSelecteerKlant??5   	   $	??450??60   $	??800??6=   .??5	      ? ???	#FFE5E5E5 ? RowDefinitions 5
   
   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $??* ?6   ?? 5      $
??auto ?6    ? ColumnDefinitions 5   
   ?? 5      $
??auto ?6   ?? 5      $??* ?6   ?? 5      $
??auto ?6   .+?5   
   ?? $??0??6   $??0??6   $??Klant Id??6.   $??Center=?6A   $??5q?6\   ?? 5   
   -   5        $??KlantIdTextBoxAanpassen??5   0   $??0??6   $??1??6    $??5q?6O   ?? 5   
   $??1??6   $??0??6   $
??Naam??6.   $??Center=?6=   $??5q?6X   ?? 5   
   -   5        $??NaamTextBoxAanpassen??5   0   $??1??6   $??1??6    $??5q?6L   ?? 5   
   $??2??6   $??0??6   $??Adres??6-   $??Center=?6=   $??5q?6X   ?? 5   
   -   5        $??AdresTextBoxAanpassen??5   /   $??2??6   $??1??6   $??5q?6L   ?? 5   
   -   5        $??	ZoekKlant??5   E   $??3??6   $??3??6!   $??  Zoek  ??62   $??5q?6V   6a   ?? 5   
   -   5        $??KlantenListBox??5   3   $??3??6   $??3??6   $??5q?6I    ?? SelectionMode$ Single=?6T    ? ContextMenu 5      ?? 5      .@?5       m? -   5        $??Delete??5       6N   m? 5!      -   5        $??Update??5!      6N   m? 5"      -   5        $	??New??5"      6H   5#      5%      ?? 5&   
   -	   5        $??SelecteerKlant??5'      $??4??5&      $??3??6   $??  Selecteer Klant  ??62   $??5q?5'   +   ${?False??5(   2   5)      5*      