using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameMng : MonoBehaviour
{
    public enum EmSwingState
    {
        None,
        Strike,
        Spare,
    }

    public UIInput ins_inputScore = null;
    public UILabel ins_labelScore = null;
    public UILabel[] ins_labelFrameRecods = null;
    public UILabel[] ins_labelTotalScores = null;

    //
    private int _nCurFrame = 1;
    private int _nTotalScore = 0;
    //볼링핀 남은갯수.
    private int _nPinReamin = 10;
    private List<CFrameInfo> _listCFrameInfos = new List<CFrameInfo>();
    //

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < CConfig.M_nFrameMaxCount; i++)
        {
            _listCFrameInfos.Add(new CFrameInfo());
        }
        ResetData();
    }

    private void ResetData()
    {
        for (int i = 0; i < _listCFrameInfos.Count; i++)
        {
            _listCFrameInfos[i].Clear();
        }
        for (int i = 0; i < ins_labelFrameRecods.Length; i++)
        {
            ins_labelFrameRecods[i].text = string.Empty;
            ins_labelTotalScores[i].text = string.Empty;
        }
        ins_labelScore.text = string.Empty;
        _nCurFrame = 1;
        _nTotalScore = 0;
        _nPinReamin = CConfig.M_nPinMaxCount;
    }

    private void SetScore(int nInputScore)
    {
        CFrameInfo cCurFrameInfo = _listCFrameInfos[_nCurFrame - 1];
        if (cCurFrameInfo.m_nChance <= 0)
        {
            SetText("게임이 종료됐습니다. 다시 시작해주세요.");
            return;
        }

        //핀 감소.
        if (_nPinReamin < nInputScore)
            nInputScore = _nPinReamin;
        _nPinReamin -= nInputScore;
        //

        bool bLastFrame = _nCurFrame == CConfig.M_nFrameMaxCount;

        //현재 프레임 정보 갱신.
        EmSwingState emSwingState = GetSwingState(nInputScore, _nPinReamin, cCurFrameInfo.m_nChance, bLastFrame);
        cCurFrameInfo.SetFrameInfo(bLastFrame, nInputScore, emSwingState);
        //

        //UI갱신.
        SetRecodUI(_nCurFrame, cCurFrameInfo.m_emSwingState, nInputScore, cCurFrameInfo.IsFinishFrame());
        SetFrameInfo(nInputScore);
        //

        Debug.Log(string.Format("남은 횟수:{0} 쓰러트린 핀:{1} 남은 핀:{2} 현재 프레임:{3} 남은 보너스:{4}"
            , cCurFrameInfo.m_nChance, nInputScore, _nPinReamin, _nCurFrame, cCurFrameInfo.m_nBonusSwing));

        bool bNextFrame = false;
        if (cCurFrameInfo.IsFinishFrame())
        {
            if (cCurFrameInfo.m_nChance <= 0 && bLastFrame)
            {
                if (bLastFrame && !cCurFrameInfo.m_bRecordedScore)
                    _nTotalScore += cCurFrameInfo.m_nTotalScore;

                SetText("게임 종료!! 최종점수 : " + _nTotalScore);
                UpdateFrameScoreUI(ins_labelTotalScores[_nCurFrame - 1], _nTotalScore);
                return;
            }

            //프레임 증가.
            if (_nCurFrame < CConfig.M_nFrameMaxCount)
            {
                _nCurFrame++;
                bNextFrame = true;
            }
        }

        if (_nPinReamin <= 0 || bNextFrame)
            _nPinReamin = CConfig.M_nPinMaxCount;
    }

    private void SetFrameInfo(int nSwingPinCount)
    {
        for (int i = 0; i < _nCurFrame; i++)
        {
            CFrameInfo cFrameInfo = _listCFrameInfos[i];
            if (cFrameInfo.m_bRecordedScore)
                continue;

            bool bRecordScore = true;

            if (i < _listCFrameInfos.Count)
            {
                switch (cFrameInfo.m_emSwingState)
                {
                    case EmSwingState.Strike:
                    case EmSwingState.Spare:
                        if (cFrameInfo.IsFinishFrame())
                        {
                            if (cFrameInfo.m_nBonusSwing > 0 && i != _nCurFrame - 1)
                            {
                                cFrameInfo.SetScore(nSwingPinCount);
                                cFrameInfo.m_nBonusSwing--;
                            }
                        }
                        bRecordScore = cFrameInfo.m_nBonusSwing <= 0;
                        break;
                    case EmSwingState.None:
                        bRecordScore = cFrameInfo.IsFinishFrame();
                        break;
                }
            }

            if (bRecordScore)
            {
                //UI갱신.
                _nTotalScore += cFrameInfo.m_nTotalScore;
                UpdateFrameScoreUI(ins_labelTotalScores[i], _nTotalScore);
            }
            cFrameInfo.m_bRecordedScore = bRecordScore;
        }
    }

    private void UpdateFrameScoreUI(UILabel uILabel, int nScore)
    {
        uILabel.text = nScore.ToString();
    }

    private void SetRecodUI(int nCurFrame, EmSwingState emSwingState, int nAddScore, bool bNextFrame)
    {
        string strFrameRecod = string.Empty;
        switch (emSwingState)
        {
            case EmSwingState.Strike:
                strFrameRecod += "X";
                break;
            case EmSwingState.Spare:
                strFrameRecod += "/";
                break;
            default:
                if (nAddScore <= 0)
                {
                    strFrameRecod += "-";
                    break;
                }
                strFrameRecod = nAddScore.ToString();
                break;
        }

        if (!bNextFrame)
        {
            strFrameRecod += "|";
        }
        ins_labelFrameRecods[nCurFrame - 1].text += strFrameRecod;
    }

    private EmSwingState GetSwingState(int nSwingPin, int nRemainPin, int nChance, bool bLastFrame)
    {
        if (nRemainPin > 0)
            return EmSwingState.None;

        bool bMaxCountPin = nSwingPin == CConfig.M_nPinMaxCount;
        if (bLastFrame)
        {
            return bMaxCountPin ? EmSwingState.Strike : EmSwingState.Spare;
        }
        return bMaxCountPin && nChance >= CConfig.M_nMaxChance ? EmSwingState.Strike : EmSwingState.Spare;
    }

    private void SetText(string strText)
    {
        ins_labelScore.text = strText;
    }

    //btn call.
    public void OnClickInputScore()
    {
        if (ins_inputScore.value == string.Empty)
            return;

        int nInputScore = int.Parse(ins_inputScore.value);
        if (nInputScore < 0 || nInputScore > 10)
        {
            SetText("잘못된 입력값 :" + nInputScore);
            return;
        }
        SetText("입력값 :" + nInputScore.ToString());

        SetScore(nInputScore);
        ins_inputScore.text = "점수입력";
    }

    public void OnClickReset()
    {
        ResetData();
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    //프레임 클래스.
    public class CFrameInfo
    {
        public EmSwingState m_emSwingState;
        public int m_nChance;
        public int m_nTotalScore;
        public bool m_bRecordedScore;
        public int m_nBonusSwing;

        public void SetFrameInfo(bool bLastFrame, int nInputScore, EmSwingState emSwingState)
        {
            SetScore(nInputScore);
            m_emSwingState = emSwingState;
            SetEmSwingState(emSwingState);
            SetChance(bLastFrame);
            SetBonus(bLastFrame);
        }

        //보너스점수 카운트 셋팅.
        private void SetBonus(bool bLastFrame)
        {
            //마지막 프레임 보너스투구에는 보너스투구를 추가 하지않는다.
            if (bLastFrame && m_nBonusSwing > 0)
                return;

            m_nBonusSwing = GetSwingStateCount(m_emSwingState);
        }

        private void SetEmSwingState(EmSwingState emSwingState)
        {
            m_emSwingState = emSwingState;
        }

        private void SetChance(bool bLastFrame)
        {
            if (bLastFrame && m_emSwingState != EmSwingState.None && m_nBonusSwing <= 0)
            {
                m_nChance = GetSwingStateCount(m_emSwingState);
            }
            else if (!bLastFrame && m_emSwingState != EmSwingState.None)
            {
                m_nChance = 0;
            }
            else
            {
                m_nChance--;
            }
        }

        public void SetScore(int nAddScore = 0)
        {
            AddTotalScore(nAddScore);
        }

        //스트라이크는 2개, 스페어는1개.
        private int GetSwingStateCount(EmSwingState emSwingState)
        {
            if (emSwingState == EmSwingState.None)
                return 0;
            return emSwingState == EmSwingState.Strike ? 2 : 1;
        }

        private void AddTotalScore(int nAddScore = 0)
        {
            m_nTotalScore += nAddScore;
        }

        public bool IsFinishFrame()
        {
            return m_nChance <= 0;
        }

        public void Clear()
        {
            m_emSwingState = EmSwingState.None;
            m_nChance = CConfig.M_nMaxChance;
            m_bRecordedScore = false;
            m_nTotalScore = 0;
            m_nBonusSwing = 0;
        }
    }
}
