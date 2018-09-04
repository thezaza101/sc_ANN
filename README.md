# Artificial Neural Network (ANN) created for University Class

Please note that the ANN code is from the Neural Network Demo with C# by James McCaffrey @ Microsoft Build 2014.

The Code was modified by Mr Robert Cox from the University of Canberra and myself 

## How can this project help you
Make changes to your ANN inputs without changing code, this will save a lot of time and effort.

### to do list
* Make loading/working with large datasets more efficient
* All non-UI code to be run on a background thread to ensure the UI is not locked while processing
* Progress bar

## Data to run this project
I have not included the data files with this project apart from iris. You should be able to get the data files from the course site. 

## How to get this up and running

Download the .NET Core SDK from https://www.microsoft.com/net/download (this project is created for version 2.1)

Install git from https://git-scm.com/ (This is optional however highly recommended, very much worthwhile learning if you intend on becoming a software developer/data scientist)

For advanced users, enter the following in command prompt/terminal (install git/.NETCore SDK first):
* on windows you will need to add the install location of .NET core to your [path variables](https://www.java.com/en/download/help/path.xml) (usually "C:\Program Files\dotnet\")
```sh
git clone https://github.com/thezaza101/sc_ANN.git
cd sc_ANN
dotnet build
cd ui
dotnet run
```

*Note that I have not tested this method but should work*
if you do not want to install git/use command line (you still need to install the .NETCore SDK):
* Download the contents of this repo from: https://github.com/thezaza101/sc_ANN/archive/master.zip
* Unzip its contents.
* Open the .sln file using Visual studio
* Set "ui" as the startup project 

### Loading data & Running the project
if you press the 'Run All' button, it should have the same effect as the 'Iris' button on from the tute files.

Note that some of the datasets provided in class may not load into this program without pre-processing as:
* They are separated by tabs or multiple spaces instead of a single delimiter.
* They are not a pure dataset and contain information in a format that doesn’t fit the other data in the dataset.
* These errors can be fixed by opening the file in notepad and doing a find and replace/deleting inconsistent data.

### Debugging
* You have access to all the code as such debuging is the same as any other c# project
* This project runs on .NET Core instead of .NET Framework, as such a lot of the 'windows only' functionality of c# is gone, i.e. System.Windows.Forms (Win32 GUI aka Windows Forms). However it has the added benefit of being able to run on linux and OSX.
* This application follows the [Model–View–ViewModel (MVVM)](https://www.wintellect.com/model-view-viewmodel-mvvm-explained/) pattern for its UI, this is why it may seem like there is a lot of 'unnecessary' code.
* The XAML used in the UI is not the same as the XAML for WPF applications (but they are similar), Check out https://github.com/AvaloniaUI/Avalonia


## Making contributions

*For advanced users: Fork this repo, make changes in your forked repo, then create a pull request to merge with this repo.*

To propose a change, you first need to [create a GitHub account](https://github.com/join).

Once you're signed in, you can browse through the folders above and choose the content you're looking for. You should then see the content in Markdown form. Click the Edit icon in the top-right corner to start editing the content.

The content is written in the Markdown format. [There's a guide here on how to get started with it](https://guides.github.com/features/mastering-markdown/).

You can preview your changes using the tabs at the top of the editor.

When you're happy with your change, make sure to create a pull request for it using the options at the bottom of the page. You'll need to write a short description of the changes you've made.

A pull request is a proposal for a change to the content. Other people can comment on the change and make suggestions. When your change has been reviewed, it will be "merged" - and it will appear immediately in the published content.

Take a look at [this guide on GitHub about pull requests](https://help.github.com/articles/using-pull-requests/).
