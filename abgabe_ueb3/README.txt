Architektur verteilter Anwendungen Übung 2

###Erläuterung der Idee
Die klasse Deadlock.cs repräsentiert einen der N Prozesse. Da diese N Prozesse sich nur in der Reihenfolge der Operationen unterscheiden machen alle das selbe, es wird lediglich
im Konstruktor ueberprueft ob es sich um einen geraden oder ungeraden Prozess handelt und dann entsprechend Datei A mit Datei B getauscht. 

Die Verwaltung ist ein eigenes Programm das die Schreibrechte verwaltet und jedem anfragenden Prozess den aktuellen Stand meldet. Das heisst im Falle das das Schreibrecht bereits vergeben ist wird die Port nummer des Prozesses und die false fuer die Freigabe gesendet. 

###Nachrichtenformat
Das Nachrichtenformat wird durch die Klasse Message.cs repräsentiert. Diese Klasse ist serialisierbar und wird zwischen der Verwaltung und den N Prozessen verwendet. 
Als Beispiel wird hier eine Kontrollnachricht in Serialisierter Form gezeigt.

<?xml version="1.0" encoding="utf-8"?>
<Message xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <senderId>6000</senderId>
  <nachricht>filea.txt</nachricht>
  <typ>request.file</typ>
</Message>

Eine Nachricht besteht aus den Attributen:
	-senderId	Was der Portnummer des Senders entspricht. 	
	-nachricht	Der Inhalt der Nachricht. Hier steht um welches File es sich handelt.  
	-typ		Der Typ der Nachricht kann eine konstante der Message_2.cs sein. Hier kann bestimmt werden um was es sich fuer eine Anfrage handelt. 


###Erläuterung der Softwarestruktur
Die DeadlockVerwaltung.cs besteht aus der Klasse Verwaltung die verschiedene Funktionen implementiert, um zu ueberpruefen was der Anfragende Prozes gerne tun moechte. 
Die Verlwaltung laeuft auf einem festen Port und ist fuer eine bestimmte Datei verantwortlich.

Die N Prozesse werden von der Deadlock.cs repräsentiert und fuehren die in der Aufgabenstellung beschriebenen Aktionen nacheinander aus. 

Die Kommunikation wird von der Klasse Resource.cs zur verfuegung gestellt. Diese besitzt die Möglichkeit auf eine einkommende Nachricht zu warten oder sich zu einem bestimmten 
Ziel zu verbinden. Ausserdem beinhaltet sie verschiedene Informationen, wie z.b. die Ports der Verwaltungsprozesse oder den eigenen.

###Hinweise auf Implementierungsbesonderheiten
	-Allgemein
		Jeder der N Prozesse fordert solange Schreibrecht auf die erste Datei bis er diese gewaerht bekommt. Dannach fordert er das Schreibrecht fuer die 2te Datei. 
		Hier kann der Deadlock entstehen. Damit er aufgeloest werden, wird nach jeder Anfrage auf die 2te Datei ein Counter hochgezaehlt. Dieser wird nun benutzt um zu
		entscheiden ob ein Prozess dem anderen Blockierenden Prozess bescheid gibt. Das heisst je nachdem wie die Schwelle dafuer gesetzt wird (random) sagt ein Prozess 		 frueher oder spaeter bescheid.

###Typische Abläufe
 	- Zuerst vergewissern dasin portlist.txt genug ports eingetragen sind. 
	- Starten der Verwaltungsprozesse "startverwaltung.cmd"
	- Starten der N Prozesse "startprozesse.cmd"

###Fazit
Die Deadlocks treten immer dann auf wenn ein Prozess die Rechte fuer die Datei A hat und nach Datei B fragt, während ein anderer Prozess die Rechte fuer Datei B hat und nach Dabei A fragt. Das heisst es streiten sich in diesem Fall immer Prozesse mit ungerader mit denen die eine gerader nummer haben. 
