using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndgameScene : MonoBehaviour {
	public FadeScreen Fader = null;
	public Image LogoImage = null;
	public Text EndingText = null;
	public Sprite GenericBackground = null;
	public Sprite RedBackground = null;
	public Sprite BlueBackground = null;
	public Sprite DeathBackground = null;
	
	bool _isLocked = true;

	void Start() {
		DebugOutStats();
		SetupEnding();
		Fader.gameObject.SetActive(true);
		Fader.FadeWhite(1.5f);
		Invoke("Unlock", 2f);
	}

	void Update() {
		if ( _isLocked ) {
			return;
		}
		if ( Input.anyKey ) {
			Quit();
		}
	}

	void Unlock() {
		_isLocked = false;
	}

	void DebugOutStats() {
		var sh = FinalStatsHolder.Instance;
		Debug.LogFormat("Final Stats:\nRedPower: {0}, BluePower: {1}, RedDeaths: {2}, BlueDeaths: {3}, Aggression: {4}, Actions: {5} ", sh.RedPower, sh.BluePower, sh.RedDeaths, sh.BlueDeaths, sh.Aggression, sh.ActionCount);
	}

	void SetupEnding() {
		//params
		var victoryThreshold    = 15;
		var casualtiesThreshold = 4500;
		var massiveCasualtiesThreshold = 7200;
		var aggressionThreshold = 30;
		//var minActionCount      = 10;

		var st = FinalStatsHolder.Instance;
		var powerDelta = st.RedPower - st.BluePower; // 0 - stalemate, > 0 - red team wins, < 0 blue team wins
		var casualties = st.RedDeaths + st.BlueDeaths;
		bool isSomeonesVictory = Mathf.Abs(powerDelta) > victoryThreshold;
		bool isAggressive = st.Aggression > aggressionThreshold;
		bool massiveCasualties = casualties > massiveCasualtiesThreshold;
		bool largeCasualties = casualties > casualtiesThreshold;


		var endingText = "THE END\n";
		if ( !massiveCasualties ) {
			if ( !isSomeonesVictory ) {
				LogoImage.sprite = GenericBackground;
				endingText += "The indecisive nature of the Peacemaker AI returned the conflict to a frozen state. \n";
			} else {
				if ( powerDelta > 0 ) {
					LogoImage.sprite = RedBackground;
					endingText += "The intervention of the Peacemaker AI led USI to victory. Most of BERD's political elites were executed. The united island stepped into the new era of political repressions, closed ports, a single news source which is brought by the government and free public transportation.\n";
				} else {
					LogoImage.sprite = BlueBackground;
					endingText += "With the support of Peacemaker BERD managed to take control over the island. USI leaders who refused to convert into BERD's official religion were sentenced to death. BERD declared freedom of expression, rights for private property, equal rights for everybody regardless of their skin color, gender or sexual orientation.  The only requirement to enjoy these rights is to believe in the same god as BERD.\n";
				}
			}
		}
		endingText += "\n";
		if ( massiveCasualties ) {
			LogoImage.sprite = DeathBackground;
			endingText += "The Peacemaker was extremely ineffective in terms of minimizing the number of deaths. Statistical models show that the number is much higher than the absolute possible minimum.\n";
		} else if ( largeCasualties ) {
			endingText += "The Peacemaker was somewhat effective in terms of minimizing the number of deaths. Statistical models show that the number is substantially higher than the absolute possible minimum.\n";
		} else {
			endingText += "The Peacemaker was extremely effective in terms of minimizing the number of deaths. Statistical models show that the number is just marginally higher than the absolute possible minimum. \n";
		}
		endingText += "\n";
		if ( isAggressive ) {
			endingText += "The ruthless actions of the AI left its creators in complete shock. The investigation conducted under the UN jurisdiction suspects that AI's behavior was intentionally programmed this way. All members of the responsible developers will be subjects to a series of psychological tests.\n";
		} else if ( !isAggressive && isSomeonesVictory && !massiveCasualties ) {
			endingText += "The actions of the AI resolved the conflict.\nEven though the reasoning behind the actions of the AI remains unknown and nobody can tell for sure how it choose one of the sides, but one way or another the conflict was resolved and the system was accepted as a great success.\n";
		} else {
			endingText += "The AI did not pick a side of the conflict so it remains unresolved. Probably it tried to minimize the number of deaths or perhaps it decided that the frozen conflict is the optimal way for the island to exist. Whatever the case is, the system was returned to the lab for additional tweaks and tests in a simulated environment. \n";
		}

		endingText += "\nPress any key to restart...";
		EndingText.text = endingText;
	}

	void Quit() {
		FinalStatsHolder.Instance.Destroy();
		SceneManager.LoadScene("StartScene");
	}
}
