using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradeupAdministrator : MonoBehaviour
{
    /* stack 1:Scorerate
     * stack 2:Powerup
     * stack 3:Repair
     * stack 4:Speedup
     * stack 5:Multishot
    */

    private int itemStack;
    private int lvScore = 1;
    private int lvPower = 1;
    private int lvSpeed = 1;
    private int lvMultishot = 1;

    static private readonly int maxScore = 20;
    static private readonly int maxPower = 10;
    static private readonly int maxSpeed = 5;
    static private readonly int maxMultishot = 5;

    // 配列[0]は'0'ですべて固定 (lv--の値をそのままインデックスへ入れる)
    [SerializeField] private float[] paramsScorerate = new float[maxScore + 1];

    [SerializeField] private int[] paramsPower = new int[maxPower + 1];

    [SerializeField] private int paramRepair;

    [SerializeField] private int[] paramsRepeatwait = new int[maxSpeed + 1];
    [SerializeField] private float[] paramsBulletspeed = new float[maxSpeed + 1];
    //

    //
    private GameObject player;
    private GameObject status;
    private GameObject writer;

    void Start()
    {
        player = GameObject.Find("Player");
        status = GameObject.Find("StatusAdmin");
        writer = GameObject.Find("InterfaceWriter");

        status.GetComponent<StatusAdministrator>().SetScorerate(paramsScorerate[this.lvScore]);
        player.GetComponent<PlayerController>().SetPower(paramsPower[this.lvPower]);
        player.GetComponent<PlayerController>().SetRepeatwait(paramsRepeatwait[this.lvSpeed]);
        player.GetComponent<PlayerController>().SetBulletspeed(paramsBulletspeed[this.lvSpeed]);
        player.GetComponent<PlayerController>().SetMultishot(this.lvMultishot);
    }

    //
    public int GetLvScore() { return this.lvScore; }
    public int GetLvPower() { return this.lvPower; }
    public int GetLvSpeed() { return this.lvSpeed; }
    public int GetLvMultishot() { return this.lvMultishot; }
    //
    public void ItemGet()
    {
        this.itemStack++;
        if (this.itemStack > 5)
        {
            this.status.GetComponent<StatusAdministrator>().AddScore(2000);
            this.itemStack = 0;
        }
        this.writer.GetComponent<InterfaceWriter>().SetTextUpgrade(this.itemStack);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            switch (this.itemStack)
            {
                case 1:
                    if (this.lvScore < maxScore) { UpgradeScore(); }
                    break;
                case 2:
                    if (this.lvPower < maxPower) { UpgradePower(); }
                    break;
                case 3:
                    UpgradeRepair();
                    break;
                case 4:
                    if (this.lvSpeed < maxSpeed) { UpgradeSpeed(); }
                    break;
                case 5:
                    if (this.lvMultishot < maxMultishot) { UpgradeMultishot(); }
                    break;
            }
        }

        // デバッグ用
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1)) { itemStack = 1; Debug.Log(itemStack); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { itemStack = 2; Debug.Log(itemStack); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { itemStack = 3; Debug.Log(itemStack); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { itemStack = 4; Debug.Log(itemStack); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { itemStack = 5; Debug.Log(itemStack); }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            for (int i = 1; i < 5; i++) { UpgradeSpeed(); UpgradeMultishot(); }
            for (int i = 1; i < 10; i++) { UpgradePower(); }
        }
#endif
        //
    }

    private void UpgradeScore()
    {
        this.lvScore++;
        status.GetComponent<StatusAdministrator>().SetScorerate(paramsScorerate[this.lvScore]);
        UpgradeFinally();
    }
    private void UpgradePower()
    {
        this.lvPower++;
        player.GetComponent<PlayerController>().SetPower(paramsPower[this.lvPower]);
        UpgradeFinally();
    }
    private void UpgradeRepair()
    {
        status.GetComponent<StatusAdministrator>().RepairLife(paramRepair);
        UpgradeFinally();
    }
    private void UpgradeSpeed()
    {
        this.lvSpeed++;
        player.GetComponent<PlayerController>().SetRepeatwait(paramsRepeatwait[this.lvSpeed]);
        player.GetComponent<PlayerController>().SetBulletspeed(paramsBulletspeed[this.lvSpeed]);
        UpgradeFinally();
    }
    private void UpgradeMultishot()
    {
        this.lvMultishot++;
        player.GetComponent<PlayerController>().SetMultishot(this.lvMultishot);
        UpgradeFinally();
    }
    private void UpgradeFinally()
    {
        this.itemStack = 0;
        this.writer.GetComponent<InterfaceWriter>().SetTextUpgrade(this.itemStack);
        GameObject.Find("Speaker").GetComponent<Speaker>().SePlay(Speaker.SE.ItemUse);
    }
}
