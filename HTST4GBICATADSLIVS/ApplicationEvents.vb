Namespace My

    ' Les événements suivants sont disponibles pour MyApplication :
    ' 
    ' Startup : déclenché au démarrage de l'application avant la création du formulaire de démarrage.
    ' Shutdown : déclenché après la fermeture de tous les formulaires de l'application. Cet événement n'est pas déclenché si l'application se termine de façon anormale.
    ' UnhandledException : déclenché si l'application rencontre une exception non gérée.
    ' StartupNextInstance : déclenché lors du lancement d'une application à instance unique et si cette application est déjà active. 
    ' NetworkAvailabilityChanged : déclenché lorsque la connexion réseau est connectée ou déconnectée.
    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup
            Dim args As String = ""
            For Each arg As String In My.Application.CommandLineArgs
                args = args & arg
            Next

            If args.Length > 3 Then
                My.Settings.name_of_the_network_card = args
                My.Settings.Save()
            End If

        End Sub
    End Class


End Namespace

