# SetFolderType

[![image](https://user-images.githubusercontent.com/79026235/152910441-59ba653c-5607-4f59-90c0-bc2851bf2688.png)Download the zip file](https://github.com/LesFerch/SetFolderType/releases/download/1.0.0/SetFolderType.zip)

## Set Windows Explorer folder types using Desktop.ini files

This program adds a right-click context menu to Windows Explorer that allows you to set the folder type for an entire folder tree by creating (or editing existing) **desktop.ini** files in each folder.

The added context menu is created with registry entries only and simply provides submenus entries for each folder type. When one of those items are selected, SetFolderType.exe is run with the appropriate arguments to set the selected folder type for the selected folder tree.

This program does NOT create a context menu handler. That is, there is no code that runs when you right-click a folder. Code only runs when you actually select an action (i.e. select a folder type that you want applied). SetFolderType will add no overhead to your context menu, other than the insignificant impact of one more context menu item.

## Why set folder types using Desktop.ini files?

### Background Information

Windows supports setting the folder type to one of **General items**, **Documents**, **Pictures**, **Music**, or **Videos** with the appropriate entry in a **hidden**, **system** file named **desktop.ini**. This has the advantage of making the folder type setting permanent to the folder itself. You can reset your profile, create a new user, or reset Windows, and the folder types will be remembered and displayed with whatever view you have chosen for that folder type.

**Note**: To globallly set your preferred folder type views, please see [WinSetView](https://lesferch.github.io/WinSetView/)

However, there is a catch. Explorer only provides this functionality on drives that mount as **Local Disk** (aka "fixed disk") and are formatted **NTFS**. Therefore, this tool will work on all internal drives that are formatted **NTFS**, but it will only work on some USB drives. For example, SanDisk USB SSD drives mount as type **Local Disk**, so they will work with this tool if the drive is formatted to **NTFS**.

### Partial Fix for Windows 11

For **Windows 11** this tool provides a partial fix for some broken funtionality in the Windows 11 Explorer. Specifically, the feature **Also apply this template to all subfolders** is completely broken in Windows 11 (assuming you have not patched Windows 11 to run the Windows 10 Explorer). Here's a [video](https://www.youtube.com/watch?v=U5eEFNZEWZg) demonstrating the issue.

The fix is partial because, as noted above, it only applies to drives of type **Local Disk** that are formatted **NTFS**. For such drives, SetFolderType will give you back the ability to set the folder type for an entire folder tree.

### Permanent folder type setting for both Windows 10 and Windows 11

For **Windows 10** (or older) or [Windows 10 Explorer on Windows 11](https://lesferch.github.io/OldExplorer/), the **Also apply this template to all subfolders** feature works fine, but there is still a scenario wher you may want this tool.

As noted in the **Background** section above, setting the folder type via **desktop.ini** files makes the folder type setting permanent because it's part of the folder, instead of just a registry setting.

# How to Download and Install

1. Download the zip file using the link above.
2. Extract **SetFolderType.exe**.
3. Right-click **SetFolderType.exe**, select Properties, check **Unblock**, and click **OK**.
4. Move **SetFolderType.exe** to the folder of your choice (you need to keep this file).
5. Double-click **SetFolderType.exe** to open the Install/Remove dialog and click **Yes** to install.
6. If you skipped step 3, then, in the SmartScreen window, click More info and then Run anyway.
7. Click **OK** when the **Done** message boc appears.

**Note**: Some antivirus software may falsely detect the download as a virus. This can happen anytime you download a new executable and may require extra steps to whitelist the file.

# How to Use

Right-click any folder and you should see the SetFolderType context menu:

![image](https://github.com/LesFerch/SetFolderType/assets/79026235/353cb8a6-5d36-41cb-aecc-8051815c3a01)

\
\
[![image](https://user-images.githubusercontent.com/79026235/153264696-8ec747dd-37ec-4fc1-89a1-3d6ea3259a95.png)](https://github.com/LesFerch/SetFolderType)
