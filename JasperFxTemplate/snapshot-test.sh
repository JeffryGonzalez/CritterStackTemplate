#! /bin/bash

dotnet new install ./JasperFxTemplate

dotnet template-authoring verify critterapi


dotnet new uninstall critterapi
