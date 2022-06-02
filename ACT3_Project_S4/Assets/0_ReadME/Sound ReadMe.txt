Système de gestion du son :

	Option 1 :
		Dans chaque scène il y a un élément SoundController soit poser sur un empty normer soundController ou GameController
		Il possède 2 variable accessible de type list dans lesquels doivent être glissé les gameobjects contenants les AudioSources
		Musique et Sound effect sont dissocier.
		
	/!\ Dans le cas ou les sons sont directement sur un prefab des list static sont disponible pour quand même il réglé le son a mettre dans le start
	
	Option 2 :
		Pour les prefabs :
		
			// Ajoute les sons dans les listes pour être actualisé au prochain changement
			// Pour les musiques
			GlobalSoundController.addMusicToList(L'AudioSource);
			// Pour les effets sonore
			GlobalSoundController.addSonoreEffectToList(L'AudioSource);
			
			// Force l'actualisation, a utilé après la fonction précédente (Quand le prefab spawn il se peut que le son soit pas actualisé automatiquement donc a utilisé en complément par sécurité)
			// Pour les musiques
			GlobalSoundController.forceUpdateMusic(L'AudioSource);
			// Pour les effets sonore
			GlobalSoundController.forceUpdateSonoreEffect(L'AudioSource);
			