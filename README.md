# SetFolderType

![image](https://github.com/LesFerch/SetFolderType/assets/79026235/17bc7a7e-d77b-4720-9d22-68c6fd7e13da)

[![image](https://user-images.githubusercontent.com/79026235/152910441-59ba653c-5607-4f59-90c0-bc2851bf2688.png)Download the zip file](https://github.com/LesFerch/SetFolderType/releases/download/1.0.0/SetFolderType.zip)

## Set Windows Explorer folder types using Desktop.ini files

This program adds a right-click context menu to Windows Explorer that allows you to set the folder type for an entire folder tree by creating (or editing existing) **desktop.ini** files in each folder.

The context menu item is created with registry entries only and simply provides submenus entries for each folder type. When one of those items are selected, SetFolderType.exe is run with the appropriate arguments to set the selected folder type for the selected folder tree.

This program does NOT create a context menu handler. That is, there is no code that runs when you right-click a folder. Code only runs when you actually select an action (i.e. select a folder type that you want applied). SetFolderType will add no overhead to your context menu, other than the insignificant impact of one more context menu item.

## Why set folder types using Desktop.ini files?

### Background Information

Windows supports setting the folder type to one of **General items**, **Documents**, **Pictures**, **Music**, or **Videos** with the appropriate entry in a **hidden**, **system** file named **desktop.ini**. This has the advantage of making the folder type setting permanent to the folder itself. You can reset your profile, create a new user, or reset Windows, and the folder types will be remembered and displayed with whatever view you have chosen for that folder type.

**Note**: To globallly set your preferred folder type views, please see [WinSetView](https://lesferch.github.io/WinSetView/)

However, there is a catch. Explorer only provides this functionality on drives that mount as **Local Disk** (aka "fixed disk") and are formatted **NTFS**. Therefore, this tool will work on all internal drives that are formatted **NTFS**, but it will only work on some USB drives. For example, SanDisk USB SSD drives mount as type **Local Disk**, so they will work with this tool if the drive is formatted to **NTFS**.

### Partial Fix for Windows 11

For **Windows 11**, this tool provides a partial fix for some broken funtionality in the Windows 11 Explorer. Specifically, the feature **Also apply this template to all subfolders** is completely broken in Windows 11 (assuming you have not patched Windows 11 to run the Windows 10 Explorer). Here's a [video](https://www.youtube.com/watch?v=U5eEFNZEWZg) demonstrating the issue.

The fix is partial because, as noted above, it only applies to drives of type **Local Disk** that are formatted **NTFS**. For such drives, SetFolderType will give you back the ability to set the folder type for an entire folder tree.

### Permanent folder type setting for both Windows 10 and Windows 11

For **Windows 10** (or [Windows 10 Explorer on Windows 11](https://lesferch.github.io/OldExplorer/)), the feature **Also apply this template to all subfolders** works fine, but there is still a scenario wher you may want this tool.

As noted in the **Background** section above, setting the folder type via **desktop.ini** files makes the folder type setting permanent because it's part of the folder, instead of just a registry setting.

# How to Download and Install

**Note**: Some antivirus software may falsely detect the download as a virus. This can happen any time you download a new executable and may require extra steps to whitelist the file.

1. Download the zip file using the link above.
2. Extract **SetFolderType.exe**.
3. Right-click **SetFolderType.exe**, select Properties, check **Unblock**, and click **OK**.
4. Move **SetFolderType.exe** to the folder of your choice (you need to keep this file).
5. Double-click **SetFolderType.exe** to open the Install/Remove dialog and click **Yes** to install.
6. If you skipped step 3, then, in the SmartScreen window, click **More info** and then **Run anyway**.
7. Click **OK** when the **Done** message box appears.

![image](https://github.com/LesFerch/SetFolderType/assets/79026235/29ea99bd-201b-48cb-896a-cd85b4242088)

![image](https://github.com/LesFerch/SetFolderType/assets/79026235/500adf4d-b7f6-447b-ab13-5d8e580a7f96)

**Note**: Sometimes the **Done** message pops up behind another open window. If that happens, you should see the SetFolderType icon on the taskbar, where you can click to bring that dialog to the front. Alternativey, you can minimize the window(s) that are on top of the dialog.

# How to Use

Right-click any folder and you should see the SetFolderType context menu:

![image](https://github.com/LesFerch/SetFolderType/assets/79026235/17bc7a7e-d77b-4720-9d22-68c6fd7e13da)

Select the folder type you want applied to the folder. This will create (or edit any existing) **desktop.ini** file in the selected folder and ALL of its subfolders, all the way down the tree.

## Patience!

Although SetFolderType is very fast at populating all the subfolders with **desktop.ini** files, it's up to Explorer to update the view. Explorer does that in that background as it notices the addition (or change) of the desktop.ini files. How long it will take is variable. It depends on the speed of the computer, what other processes are running, how many folders are affected, and so forth. But it can often take 30 seconds or so for all the folder views to update. So, before you jump onto GitHub and post an issue, relax, do something else for a minute and then go back and check your folders. You should see that Explorer did its thing and updated the view. 

## The view won't change if you're looking at it!

Explorer will not update the folder's view until the folder is closed. However, having an open folder only stops the view update for that particular folder level. The subfolder's views will update even if you have the parent folder open.

## The target folder must be on a Local Disk that's formatted NTFS

If the target folder is not both a **Local Disk** and **NTFS** an error message, indicating which criteria are met (or not) will be displayed. Here's all three error possibilities:
![image](https://github.com/LesFerch/SetFolderType/assets/79026235/b5acfe58-ed9a-416c-912d-922b4d1c6c10)  ![image](https://github.com/LesFerch/SetFolderType/assets/79026235/b3381f2e-07ee-485e-b6c6-ab19e66f115f)  ![image](https://github.com/LesFerch/SetFolderType/assets/79026235/0b1a0d4d-6e27-445f-91c1-f819bcef745f)

## It's Multilingual!

Here's an example of SetFolderType installed for German (de-DE):
![image](https://github.com/LesFerch/SetFolderType/assets/79026235/339b4cd1-026d-40cd-ab8d-8eabb115996d)

SetFolderType will detect your Windows language and use it, as long as it can find the **Shell32.dll.mui** file for your current language. If that file is missing, you will see this error:
![image](https://github.com/LesFerch/SetFolderType/assets/79026235/21294f1a-7914-4886-a131-53b2e7073ed9)

If you get that error, you may click **OK** and continue to install the tool with English context menu entries. If later the missing file issue is resolved, you can simply double-click **SetFolderType.exe** again to install it in the correct language. There is no need to remove first. The install always removes any of its previous context menu entries before creating new ones.

If the MUI file is found, the Install/Remove dialog will appear in the correct language:
![image](https://github.com/LesFerch/SetFolderType/assets/79026235/b0d72b29-3721-47e1-9238-ef42a8346034)
\
\
[![image](https://user-images.githubusercontent.com/79026235/153264696-8ec747dd-37ec-4fc1-89a1-3d6ea3259a95.png)](https://github.com/LesFerch/SetFolderType)
