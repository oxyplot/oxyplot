The Xamarin Component is generated from sh scripts executed on a Mac with Xamarin Studio installed. 

# Scripts

./make-component.sh         cleans, builds, creates documentation and creates the component.
  ./clean.sh                cleans the output files
  ./build.sh                updates version numbers*, builds OxyPlot, OxyPlot.XamarinIOS and OxyPlot.XamarinAndroid
  ./create-doc.sh           creates the monodoc, html and msxdoc documentation files
  ./create-component.sh     creates the component

./test-component.sh         tests building the samples of the component
./show-doc.sh               shows the monodoc documentation using macdoc

* the version number is found from the latest OxyPlot.Core NuGet package

The xam package will be found in the ~/Output/ folder when the scripts are completed.

# monodoc documentation generation
mdoc.exe is used to generate documentation from the inline xml code comments.

http://www.mono-project.com/Monodoc
http://www.mono-project.com/Generating_Documentation
http://www.slideshare.net/migueldeicaza/using-monodoc