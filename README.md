# helppolasku-mvvm
Invoicing app

A friend of mine asked me to develop an easy-to-use and simple invoicing application. I wanted to learn MVVM-pattern and took the challenge. I guess I should try to get rid of all the hardcoded strings next and learn how to do localization ;)

I implemented pdf creation with MigraDoc library: http://www.pdfsharp.net/

Note that user created content is not stored anywhere in this version, but feel free to code your own dataservices as bottom layer for the existing DAL with IDataHandler interface.