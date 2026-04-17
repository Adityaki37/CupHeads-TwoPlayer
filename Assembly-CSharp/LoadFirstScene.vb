Imports System
Imports System.Collections
Imports UnityEngine.SceneManagement

' Token: 0x02000442 RID: 1090
Public Class LoadFirstScene
	Inherits AbstractMonoBehaviour

	' Token: 0x06001003 RID: 4099 RVA: 0x0009E633 File Offset: 0x0009CA33
	Private Sub Start()
		SceneManager.LoadScene(0)
	End Sub

	' Token: 0x06001004 RID: 4100 RVA: 0x0009E63C File Offset: 0x0009CA3C
	Private Iterator Function load_cr() As IEnumerator
		Yield Nothing
		Return
	End Function
End Class
