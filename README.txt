Architektur verteilter Anwendungen Übung 1 

###Erläuterung der Idee
Die Klasse Knoten.cs in übernimmt die Aufgabe eines Knoten innerhalb eines Graphens. Dieser besitzt alle Methoden die dafür erforlderlich sind.



###Nachrichtenformat
Das Nachrichtenformat wird durch die Klasse Message.cs repräsentiert. Diese Klasse ist serialisierbar und wird zwischen den Knoten verendet. 
Als Beispiel wird hier eine Kontrollnachricht in Serialisierter Form gezeigt.

<?xml version="1.0" encoding="utf-8"?>
<Message xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <senderId>-1</senderId>
  <nachricht>Nachricht</nachricht>
  <typ>ctrl</typ>
</Message>

Eine Nachricht besteht aus den Attributen:
	-senderId	Die Id des Knoten der diese Nachricht gesendet hat/ Wenn die Id von dem Benutzer gesendet wird ist diese -1
	-nachricht	Der Inhalt der Nachricht. Bei einer KontrollNachricht sind drei Befehle zulässig: endall, end, init
	-typ		Der Typ der Nachricht, Aktuell unterscheidet ein Knoten zwischen Kontrollnachrichten (ctrl) und normalen Nachrichten (msg)

###Erläuterung der Softwarestruktur
Die Gesamte Übung umfasst 3 Programme.
	-Knoten		
		Das Knoten Programm umfasst dabei die Klassen Knoten.cs, Programm.cs und Message.cs
		Die Knotenklasse stellt verschiedene Funktionen zum einlesen von Informationen bereit, 
		wie das einlesen einer Graphviz-Datei oder einfach eine Liste von Informationen nach dem Schema "id Hostname:Port"
		Um die KnotenKlasse zu starten und die args über die Kommandozeile einzulesen existeirt die Klasse Programm.cs die
		über die main-Methode des Programms verfügt. Die Klasse Message.cs wird dafür verwendet um Iformationen zwischen Klassen
		zu transportieren. 	

	-Steuerung
		Das Programm Steuerung der zur manuellen Steruerung der Knoten. Das Programm verfügt über eine main-Methode die die args der Kommandozeile einliest
		und eine Verbindung zum gewünschten Knoten herstellt und diesem eine Nachricht sendet.
		
	-Graphgen
		Das Programm Graphgen erstellt einen graphizdatei. 


###Hinweise auf Implementierungsbesonderheiten
Die Kommunikatioen zwischen den Knoten findet über TCP statt, wobei die Verbindung direkt nach übertragen der Nachricht beendet wird. 

	-Grapghen
		Das Graphgen-Programm erstellt als erstes einen Graphen mit "Knotenanzahl-1 Kanten", damit der Graph auf jedenfall zusammenhägend ist. 
		Damit die Kantenanzahl größer als die Knoteanzahl ist wird sich die Differnz der von den zu erstellenden Kanten zu den erstellten Kanten gemerkt.
		Das heisst die "zu erstellende Knoten" - "Knotenanzahl-1". Im anschluss wird eine Zweite Schleife über diese Differenz iterieren und erstellt beliebige Kanten.
		Am Ende werden die Doppelten Kanten entfernt. Daher kann es sein das weniger Kanten erstellt werden als über die args angegeben wird.

###Typische Abläufe
Um mehrere Knoten zu Starten kann das Skript start.cmd ausgeführt werden mit den Parametern wie viele Knoten erstellt werden sollen
	- start.cmd "Knotenanzahl" "Belivecounter für Knoten"

Damit die Knoten wissen über welchen port, ip und id sie verfügen kann das Skript gentest.cmd ausgeführt werden. Es schreibt die Informationen "id localhost:port" in die test.txt#
	- gentest.cmd 25  --> erstellt 25 einträge in der test.txt


###Fazit
Ursprünglich war geplant das Programme über das mono-project auf einem Mac laufen zu lassen. Bis zu einer gewissen Knotenanzahl war dies auch Möglich. Allerdings treten bei
großen Graphen übertragungsprobleme auf. Daher empfinde ich die Entwicklung von C# Projekten unter Windows sehr viel angenehmer da dort die IDE Visual Studio verfügbar ist. 
Ausserdem habe denke ich, das das serialiseren mit XML sich durchaus einfacher realisieren lässt als mit JSON. Diese Folge ziehe ich daraus das ich bereits in der Bachelorthesis damit zu tun hatte. 
Zu dem mono-Project kann allerdings gesgat werden das es eine tolle idee ist und da .Net seit kurzem Open-Source ist wird sich in diese Richtung noch viel tun. 
Da ich bisher nur einige Datenbankabfragen mit C# realisiert habe und dies das erste Programm ist das ich bisher mit C# geschrieben habe muss ich sagen das die Entwicklung schnell geht und sehr einfach ist. Ausserdem gibt es eine sehr große und ausführliche Dokumentation zu der Sprache. 
