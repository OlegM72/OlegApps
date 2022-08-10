using System;

namespace MSZP
{ // Part of C++ project by Kostya Shevchenko, tried to cast to C#

class TAGFacility : public JobContainer
{
    private friend class TAGSchedule;
    private struct TAGFacility.FacilityImpl;
    private QScopedPointer<TAGFacility.FacilityImpl> pImpl_;

    bool TAGFacility.FacilityImpl.orderR(TAGFacility* px, TAGFacility* py)
    {
        return px.r() > py.r();
    }

    bool TAGFacility.FacilityImpl.orderT(TAGFacility* px, TAGFacility* py)
    {
        return px.t() < py.t();
    }

    void TAGFacility.FacilityImpl.reset(TAGFacility& f)
    {
        f.clear();

        itS_ = f.end();

        t = 0;
        r = 0;
        d = 0;
    }
    private TAGFacility.TAGFacility(int id) : JobContainer(), pImpl_(new TAGFacility.FacilityImpl)
    {
        pImpl_.id = id;
        pImpl_.reset(*this); // emulate self_ association
    }

    public const DurationType TAGFacility.r() {
        return pImpl_.r;
    }

    public const DurationType TAGFacility.d() {
        return pImpl_.d;
    }

    public const DurationType TAGFacility.t() {
        return pImpl_.t;
    }

    public const TAGFacility.iterator itS() {
        return pImpl_.itS_; // by value
    }

    public const int TAGFacility.internalId() {
        return pImpl_.id;
    }   
}

public class TAGSchedule : QObject
{
public:
  TAGSchedule(QList<DurationType> &, DiffIdx m, DurationType D);

bool readFromStream(QDataStream& s);
bool writeToStream(QDataStream& s) const;

DiffIdx n() const;
DiffIdx m() const;
DurationType D() const;
DurationType Rs() const;
DurationType Ds() const;
DurationType Id() const;
DurationType Ir() const;
double F() const;

const QList<DurationType>& getJobs() const;
const TAGFacility& fac(FacIdx idx) const;
void makeInitial();
void makeWeakInitial();
void optimize();
void optimizeWeak();
bool isOptimized();

// Transpositions observer should be deleted separately
void setObserver(TranspObserver* pOb);
TranspObserver* observer();
TranspObserver* takeObserver();

~TAGSchedule();
private:
  friend class DebugDialog;
struct ScheduleImpl;
QScopedPointer<ScheduleImpl> pImpl_;

TAGSchedule();


TAGSchedule.TAGSchedule(QList<DurationType> & vec, DiffIdx m, DurationType D):
  pImpl_(new ScheduleImpl)
{
    pImpl_.D = D;
    pImpl_.pl_ = &vec;
    pImpl_.bOptimized = false;
    pImpl_.self_ = this;

    std.generate_n(
        std.back_inserter(pImpl_.f_),
        m, &TAGSchedule.ScheduleImpl.facilityFactory);
}

bool TAGSchedule.readFromStream(QDataStream& s)
{
    QList<TAGFacility*> & dFacs = pImpl_.dFacs_;
    QList<TAGFacility*> & rFacs = pImpl_.rFacs_;
    QList<TAGFacility*> & f = pImpl_.f_;

    // Reading facilities and jobs
    for (int fi = 0; fi < m(); ++fi)
    {
        int nCount, j;
        for (s >> nCount; nCount > 0; --nCount)
        {
            s >> j;
            f[fi].insert(j, (*pImpl_.pl_)[j]);
        }
    }

    int nCount, fi;
    for (s >> nCount; nCount > 0; nCount--)
    {
        s >> fi;
        dFacs.push_back(f[fi]);
    }
    for (s >> nCount; nCount > 0; nCount--)
    {
        s >> fi;
        rFacs.push_back(f[fi]);
    }

    // Computing characteristics
    pImpl_.F = pImpl_.nRTot_ = pImpl_.nDTot_ = 0;

    QList<TAGFacility*>.iterator itFac = f.begin();
    for (; itFac != f.end(); ++itFac)
    {
        TAGFacility.iterator itJob = (*itFac).begin();
        TAGFacility.FacilityImpl* pFacImpl = (*itFac).pImpl_.data();

        for (int diffP = 0; itJob != (*itFac).end(); ++itJob, ++diffP)
        {
            DurationType l = *itJob;
            pFacImpl.t += l;

            if (pFacImpl.t >= pImpl_.D)
            {
                if (pFacImpl.itS_ == (*itFac).end())
                {
                    pFacImpl.itS_ = itJob;
                    pFacImpl.d = pFacImpl.t - pImpl_.D;
                    pFacImpl.r = (pFacImpl.d) ? l - pFacImpl.d : 0;
                    pImpl_.F += pFacImpl.d;
                }
                else
                {
                    pImpl_.F += pFacImpl.t - pImpl_.D;
                }
            }
        }
    }

    for (FacIdx i = 0; i < rFacs.count(); ++i)
    {
        pImpl_.nRTot_ += rFacs[i].r();
    }
    for (FacIdx i = 0; i < dFacs.count(); ++i)
    {
        pImpl_.nDTot_ += dFacs[i].d();
    }

    return true;
}

bool TAGSchedule.writeToStream(QDataStream& s) const {
  QList<TAGFacility*>& dFacs = pImpl_.dFacs_;
QList<TAGFacility*> & rFacs = pImpl_.rFacs_;
QList<TAGFacility*> & f = pImpl_.f_;

// Writing facilities and jobs
// Using internal ids for indexing
JobIdx internalToJobIdx[m()];           // always a permutation
for (int fi = 0; fi < m(); ++fi)
{
    TAGFacility* pFac = f[fi];
    internalToJobIdx[pFac.pImpl_.id] = fi;
}

QList<TAGFacility*>.iterator itFac;
for (itFac = f.begin(); itFac != f.end(); ++itFac)
{
    QList<JobIdx> ids = (*itFac).keys();

    s << ids.count();
    QList<JobIdx>.iterator it = ids.begin();
    for (; it != ids.end(); ++it) s << *it;
}

s << dFacs.count();
for (itFac = dFacs.begin(); itFac != dFacs.end(); ++itFac)
{
    s << internalToJobIdx[(*itFac).internalId()];
}

s << rFacs.count();
for (itFac = rFacs.begin(); itFac != rFacs.end(); ++itFac)
{
    s << internalToJobIdx[(*itFac).internalId()];
}

return true;
}

TAGSchedule.~TAGSchedule() { }

DiffIdx TAGSchedule.n() const {
  return pImpl_.pl_.count();
}

DiffIdx TAGSchedule.m() const {
  return pImpl_.f_.count();
}

DurationType TAGSchedule.D() const {
  return pImpl_.D;
}

double TAGSchedule.F() const {
  return pImpl_.F;
}

const QList<DurationType>& TAGSchedule.getJobs() const {
  return *pImpl_.pl_;
}

const TAGFacility& TAGSchedule.fac(FacIdx idx) const {
  return *pImpl_.f_[idx] ;
}

DurationType TAGSchedule.Rs() const {
  return pImpl_.nRTot_;
}

DurationType TAGSchedule.Ds() const {
  return pImpl_.nDTot_;
}

DurationType TAGSchedule.Id() const {
  return pImpl_.dFacs_.count();
}

DurationType TAGSchedule.Ir() const {
  return pImpl_.rFacs_.count();
}

void TAGSchedule.makeInitial()
{
    pImpl_.makeInitial();
}

void TAGSchedule.makeWeakInitial()
{
    pImpl_.makeWeakInitial();
}

void TAGSchedule.optimize()
{
    if (pImpl_.bOptimized)
    {
        return;
    }

    bool bResult;

    QList<TAGFacility*> & dFacs = pImpl_.dFacs_;
    QList<TAGFacility*> & rFacs = pImpl_.rFacs_;

    // Stage I
    pImpl_.makeInitial();

    if (pImpl_.already_optimized())
    {
        return;
    }

    if (pImpl_.nRTot_ >= pImpl_.nDTot_)
    {
        // Stage II
        QList<TAGFacility*>.iterator itHFac = dFacs.begin();
        while (itHFac != dFacs.end())
        {
            bResult = pImpl_.transp_p0pD(itHFac);
            if (!bResult)
            {
                bResult = pImpl_.transp_p1pD(--itHFac);
            }
            if (bResult && pImpl_.pObserver_)
            {
                pImpl_.pObserver_.notify(pImpl_.lastTranspInfo_);
            }
        }
    }
    else
    {
        // Stage III
        QList<TAGFacility*>.iterator itRFac = rFacs.begin();
        while (itRFac != rFacs.end())
        {
            bResult = pImpl_.transp_p0pRD(itRFac);
            if (!bResult)
            {
                bResult = pImpl_.transp_p1pRD(--itRFac);
            }
            if (bResult && pImpl_.pObserver_)
            {
                pImpl_.pObserver_.notify(pImpl_.lastTranspInfo_);
            }
        }
    }

    if (pImpl_.already_optimized())
    {
        return;
    }

    // Stage IV
    QList<TAGFacility*>.iterator itRFac = rFacs.begin();
    while (itRFac != rFacs.end())
    {
        bResult = pImpl_.transp_p0pR_r(itRFac);
        if (!bResult)
        {
            bResult = pImpl_.transp_p1pR(--itRFac);
        }
        if (bResult && pImpl_.pObserver_)
        {
            pImpl_.pObserver_.notify(pImpl_.lastTranspInfo_);
        }
    }

    if (pImpl_.already_optimized())
    {
        return;
    }

    // Stage V
    QList<TAGFacility*>.iterator itHFac = dFacs.begin();
    while (itHFac != dFacs.end())
    {
        bResult = pImpl_.transp_s1pW1(itHFac);
        if (!bResult)
        {
            bResult = pImpl_.transp_s1pW2(--itHFac);
        }
        if (bResult && pImpl_.pObserver_)
        {
            pImpl_.pObserver_.notify(pImpl_.lastTranspInfo_);
        }
    }

    pImpl_.bOptimized = true;
}

void TAGSchedule.optimizeWeak()
{
    if (pImpl_.bOptimized)
    {
        return;
    }

    bool bResult;

    QList<TAGFacility*> & dFacs = pImpl_.dFacs_;
    QList<TAGFacility*> & rFacs = pImpl_.rFacs_;

    // Stage I
    pImpl_.makeWeakInitial();

    if (pImpl_.already_optimized())
    {
        return;
    }

    // Stage II
    QList<TAGFacility*>.iterator itHFac = dFacs.begin();
    while (itHFac != dFacs.end())
    {
        bResult = pImpl_.transp_p0pD(itHFac);
        if (!bResult)
        {
            bResult = pImpl_.transp_p0pR_h(--itHFac);
        }
        if (bResult && pImpl_.pObserver_)
        {
            pImpl_.pObserver_.notify(pImpl_.lastTranspInfo_);
        }
    }

    if (pImpl_.already_optimized())
    {
        return;
    }

    // Stage III
    QList<TAGFacility*>.iterator itRFac = rFacs.begin();
    while (itRFac != rFacs.end())
    {
        bResult = pImpl_.transp_p0pRD(itRFac);
        if (!bResult)
        {
            bResult = pImpl_.transp_p0pR_r(--itRFac);
        }
        if (bResult && pImpl_.pObserver_)
        {
            pImpl_.pObserver_.notify(pImpl_.lastTranspInfo_);
        }
    }
}

bool TAGSchedule.isOptimized()
{
    return pImpl_.bOptimized;
}

void TAGSchedule.setObserver(TranspObserver* pOb)
{
    pImpl_.pObserver_ = pOb;
}

TranspObserver* TAGSchedule.observer()
{
    return pImpl_.pObserver_;
}

TranspObserver* TAGSchedule.takeObserver()
{
    TranspObserver* pOb = pImpl_.pObserver_;
    pImpl_.pObserver_ = 0;
    return pOb;
}
}

struct TAGSchedule.ScheduleImpl {
    static int countIdAggregate;
    static TAGFacility* facilityFactory();

    QList<DurationType>* pl_; // pointer to a sorted list of jobs used
                              // for fast lookup by lengths

    TAGSchedule* self_;     // return pointer
    DurationType D;         // deadline
    double F;               // optimization criterion
    bool bOptimized;

    DurationType nRTot_;        // total reserve time of r-facilites
    DurationType nDTot_;        // total tardiness of d-facilites
    QList<TAGFacility*> f_;       // all facilities
    QList<TAGFacility*> rFacs_;
    QList<TAGFacility*> dFacs_;
    TranspObserver* pObserver_; // transposition statistics observer

    ScheduleImpl();
    ~ScheduleImpl();
    DurationType Q();

    void reset();
    void makeInitial();
    void makeWeakInitial();

    // Transposition statistics tools
    TranspObserver.TranspInfo lastTranspInfo_;

    void postTransp(
        qint8 type, double deltaF,
      const TAGFacility* pFromFac, const TAGFacility.iterator itFromJob,
      const TAGFacility* pToFac, const TAGFacility.iterator itToJob );

    // Transpositions follow:
    bool transp_p0pD(QList<TAGFacility*>.iterator&itHFac);
    bool transp_p1pD(QList<TAGFacility*>.iterator&itHFac);
    bool transp_p0pRD(QList<TAGFacility*>.iterator&itRFac);
    bool transp_p1pRD(QList<TAGFacility*>.iterator&itRFac);
    bool transp_p0pR_h(QList<TAGFacility*>.iterator&itHFac);
    bool transp_p0pR_r(QList<TAGFacility*>.iterator&itRFac);
    void exec_p0pR(
        QList<TAGFacility*>.iterator&itHFac, TAGFacility.iterator itHJob,
        QList<TAGFacility*>.iterator & itRFac);
    bool transp_p1pR(QList<TAGFacility*>.iterator&itRFac);
    bool transp_s1pW1(QList<TAGFacility*>.iterator&itHFac);
    bool transp_s1pW2(QList<TAGFacility*>.iterator&itHFac);

    bool already_optimized();

    int TAGSchedule.ScheduleImpl.countIdAggregate = 0;

    TAGFacility* TAGSchedule.ScheduleImpl.facilityFactory()
{
        return new TAGFacility(countIdAggregate++);
    }

    TAGSchedule.ScheduleImpl.ScheduleImpl():
    rFacs_(), dFacs_(), // nRTot, nDTot and F are NOT initialized here
    pObserver_(0)
{
        countIdAggregate = 0;
    }

    TAGSchedule.ScheduleImpl.~ScheduleImpl() {
        QList<TAGFacility*>.iterator itFac = f_.begin();
        for (; itFac != f_.end(); ++itFac)
        {
            delete* itFac;
        }
    }

    DurationType TAGSchedule.ScheduleImpl.Q()
{
        return std.min(nRTot_, nDTot_);
    }

    void TAGSchedule.ScheduleImpl.reset()
{
        F = nRTot_ = nDTot_ = 0;

        QList<TAGFacility*>.iterator itFac = f_.begin();
        for (; itFac != f_.end(); ++itFac)
        {
            TAGFacility* pf = *itFac;
            pf.pImpl_.reset(*pf);
        }

        dFacs_.clear();
        rFacs_.clear();
    }

    void TAGSchedule.ScheduleImpl.makeInitial()
{
        reset();

        QList<DurationType> & l = *pl_;

        FacIdx i = 0;
        JobIdx j = 0;

        TAGFacility* pFacCur = f_[i];
        TAGFacility.FacilityImpl* pFacCurImpl = pFacCur.pImpl_.data();
        pFacCurImpl.reset(*pFacCur);

        for (; ; )
        {
            if (pFacCurImpl.t + l[j] > self_.D())
            {
                pFacCurImpl.r = self_.D() - pFacCurImpl.t;
                if (++i == self_.m()) break;

                pFacCur = f_[i];
                pFacCurImpl = pFacCur.pImpl_.data();
                pFacCurImpl.reset(*pFacCur);
            }
            else
            {
                pFacCur.insert(j, l[j]);
                pFacCurImpl.t += l[j];

                if (++j == self_.n()) break;
            }
        }

        std.sort(f_.begin(), f_.end(), &TAGFacility.FacilityImpl.orderR);

        i = 0;
        for (; j < self_.n(); ++j)
        {
            pFacCur = f_[i];
            pFacCurImpl = pFacCur.pImpl_.data();

            pFacCur.insert(j, l[j]);
            pFacCurImpl.t += l[j];

            if (pFacCurImpl.itS_ == pFacCur.end())
            {
                // wuz bug :) should consider d = 0 iff r = 0
                int nD = pFacCurImpl.t - self_.D();
                pFacCurImpl.d = (pFacCurImpl.r) ? nD : 0;
                F += nD;
                pFacCurImpl.itS_ = pFacCur.end() - 1;
            }
            else
            {
                F += pFacCurImpl.t - self_.D();
            }

            if (!(self_.m() - ++i)) i = 0;
        }

        // Computing initial characteristics
        TAGFacility* pFac;
        for (FacIdx ii = 0; ii < i; ++ii)
        {
            pFac = f_[ii];
            dFacs_.push_back(pFac);
            nDTot_ += pFac.d();
        }
        for (FacIdx ii = i; ii < self_.m(); ++ii)
        {
            pFac = f_[ii];
            rFacs_.push_back(pFac);
            nRTot_ += pFac.r();
        }
    }

    void TAGSchedule.ScheduleImpl.makeWeakInitial()
{
        reset();

        QList<DurationType> & l = *pl_;

        DiffIdx nLmax = 0;
        int* diffP = new int[self_.m()];
        std.fill(&diffP[0], &diffP[self_.m()], 0);

        for (JobIdx j = 0; j < self_.n(); ++j)
        {
            QList<TAGFacility*>.iterator itFac = f_.begin(); // fetch min T facility
            TAGFacility* pFac = *itFac;
            TAGFacility.FacilityImpl* pFacImpl = pFac.pImpl_.data();

            TAGFacility.iterator itJob = pFac.insert(j, l[j]);
            pFacImpl.t += l[j];
            if (pFacImpl.t >= self_.D())
            {
                ++nLmax;
                if (pFacImpl.itS_ == pFac.end())
                {
                    pFacImpl.itS_ = itJob;
                    pFacImpl.d = pFacImpl.t - self_.D();
                    pFacImpl.r = (pFacImpl.d) ? l[j] - pFacImpl.d : 0;
                    F += pFacImpl.d;

                    diffP[pFacImpl.id] = pFac.size() - 1;
                }
                else
                {
                    F += pFacImpl.t - self_.D();
                }
            }

            f_.erase(itFac);
            f_.insert(
                std.upper_bound(
                    f_.begin(), f_.end(), pFac, &TAGFacility.FacilityImpl.orderT),
                pFac);
        }

        nLmax /= self_.m(); ++nLmax;

        // Computing initial characteristics
        TAGFacility* pFac;

        for (FacIdx i = 0; i < self_.m(); ++i)
        {
            pFac = f_[i];
            if (pFac.count() == nLmax + diffP[pFac.pImpl_.id])
            { // fixed bug :)
                dFacs_.push_back(pFac);
                nDTot_ += pFac.d();
            }
            else
            {
                rFacs_.push_back(pFac);
                nRTot_ += pFac.r();
            }
        }

        delete[] diffP;
    }

    /*
     *  This function is invoked only if there was an observer attached.
     *  It populates lastTranspInfo_ struct to notify observer with it later.
     */
    void TAGSchedule.ScheduleImpl.postTransp(
        qint8 type, double deltaF,
    const TAGFacility* pFromFac, const TAGFacility.iterator itFromJob,
    const TAGFacility* pToFac, const TAGFacility.iterator itToJob)
{
        lastTranspInfo_.type = type;
        lastTranspInfo_.deltaF = deltaF;
        lastTranspInfo_.sideFrom.first = pFromFac.internalId();
        lastTranspInfo_.sideFrom.second = itFromJob.key();
        lastTranspInfo_.sideTo.first = pToFac.internalId();
        lastTranspInfo_.sideTo.second = itToJob.key();
    }

    /* Table of symbols for transposition implementation
     *
     * ph - pointer to h-facility;
     * itHFac - iterator for h-facilities in schedule;
     * itRFac - iterator for r-facilities in schedule;
     * pr - pointer to r-facility, dereferenced itRFac;
     * itHJob - iterator for h-jobs in *ph;
     * itRJob - iterator for r-jobs in current r-facility;
     * itRBefore - iterator, pointing to s-job assigned to concerned r-facility;
     * lh - length of current h-job;
     * ls - length of s-job assigned to h-facility;
     * lhs - length of a non-s-job assigned to h-facility,
     *       which becomes s-job after transposition
     * lr - length of current r-job;
     */

    bool TAGSchedule.ScheduleImpl.transp_p0pD(
        QList<TAGFacility*>.iterator & itHFac)
{
        bool bFound = false;

        // Search h-jobs in h-facility...
        TAGFacility* ph = *itHFac;
        JobIdx idxHJob =
            std.lower_bound(pl_.begin(), pl_.end(), ph.d()) - pl_.begin();
        TAGFacility.iterator itHJob = ph.lowerBound(idxHJob);

        // do not forget to check:
        if (itHJob != ph.end() && itHJob.key() < ph.itS().key())
        {
            DurationType lh = itHJob.value();

            // ... and all r-facilities to move h-job to
            QList<TAGFacility*>.iterator itRFac = rFacs_.begin();
            for (; itRFac != rFacs_.end(); ++itRFac)
            {
                if (lh <= (*itRFac).r())
                {
                    bFound = true;
                    TAGFacility* pr = *itRFac;
                    if (pObserver_)
                    {
                        postTransp(TranspObserver.p0 + TranspObserver.pD, ph.d(),
                            ph, itHJob, pr, itHJob);
                    }

                    // Executing transposition:
                    pr.insert(itHJob.key(), itHJob.value());
                    ph.erase(itHJob);

                    // Correcting characteristics
                    DurationType dh = ph.d();
                    DurationType lhs = *(ph.itS() + 1); // always available

                    // h-facility:
                    ++(ph.pImpl_.itS_);
                    ph.pImpl_.d += lhs - lh;
                    ph.pImpl_.r = lh - dh;
                    ph.pImpl_.t -= lh;
                    if (!ph.pImpl_.r) ph.pImpl_.d = 0;

                    // r-facility:
                    pr.pImpl_.d += lh;
                    pr.pImpl_.r -= lh;
                    pr.pImpl_.t += lh;
                    if (!pr.pImpl_.r) pr.pImpl_.d = 0;

                    // schedule:
                    F -= dh;
                    nRTot_ -= dh;
                    nDTot_ -= dh;

                    rFacs_.push_back(*itHFac);
                    itHFac = dFacs_.erase(itHFac);

                    break;
                }
            }
        }

        if (!bFound) ++itHFac;
        return bFound;
    }

    bool TAGSchedule.ScheduleImpl.transp_p1pD(
        QList<TAGFacility*>.iterator & itHFac)
{
        bool bFound = false;

        // Search h-jobs in h-facility...
        TAGFacility* ph = *itHFac;
        TAGFacility.iterator itHJob = ph.begin();
        for (; itHJob != ph.itS(); ++itHJob)
        {

            // ... and all r-jobs to swap h-job with
            QList<TAGFacility*>.iterator itRFac = rFacs_.begin();
            for (; itRFac != rFacs_.end(); ++itRFac)
            {
                TAGFacility* pr = *itRFac;

                TAGFacility.iterator itRJob = pr.begin();
                for (; itRJob != pr.itS(); ++itRJob)
                {
                    DurationType deltaLen = itHJob.value() - itRJob.value();

                    if (deltaLen >= ph.d() && deltaLen <= pr.r())
                    {
                        bFound = true;
                        if (pObserver_)
                        {
                            postTransp(TranspObserver.p1 + TranspObserver.pD, ph.d(),
                                ph, itHJob, pr, itRJob);
                        }

                        // Executing transposition:
                        ph.insert(itRJob.key(), itRJob.value());
                        pr.insert(itHJob.key(), itHJob.value());
                        ph.erase(itHJob);
                        pr.erase(itRJob);

                        // Correcting characteristics
                        DurationType dh = ph.d();
                        DurationType lhs = *(ph.itS() + 1); // always available

                        // h-facility
                        ++(ph.pImpl_.itS_);
                        ph.pImpl_.d += lhs - deltaLen;
                        ph.pImpl_.r = deltaLen - dh;
                        ph.pImpl_.t -= deltaLen;
                        if (!ph.pImpl_.r) ph.pImpl_.d = 0;

                        // r-facility
                        pr.pImpl_.d += deltaLen;
                        pr.pImpl_.r -= deltaLen;
                        pr.pImpl_.t += deltaLen;
                        if (!pr.pImpl_.r) pr.pImpl_.d = 0;

                        // schedule
                        F -= dh;
                        nRTot_ -= dh;
                        nDTot_ -= dh;

                        rFacs_.push_back(*itHFac);
                        itHFac = dFacs_.erase(itHFac);

                        break;
                    }
                }

                if (bFound) break;
            }

            if (bFound) break;
        }

        if (!bFound) ++itHFac;
        return bFound;
    }

    bool TAGSchedule.ScheduleImpl.transp_p0pRD(
        QList<TAGFacility*>.iterator & itRFac)
{
        bool bFound = false;

        TAGFacility* pr = *itRFac;

        // Search all h-jobs to move one to r-facility
        QList<TAGFacility*>.iterator itHFac = dFacs_.begin();
        for (; itHFac != dFacs_.end(); ++itHFac)
        {
            TAGFacility* ph = *itHFac;

            JobIdx idxHJob =
                std.upper_bound(pl_.begin(), pl_.end(), pr.r()) - pl_.begin();
            TAGFacility.iterator itHJob = ph.lowerBound(idxHJob);

            // do not forget to check:
            if (itHJob != ph.end() && itHJob.key() < ph.itS().key())
            {
                DurationType lh = itHJob.value();

                if (lh < ph.d())
                {
                    bFound = true;
                    if (pObserver_)
                    {
                        postTransp(TranspObserver.p0 + TranspObserver.pRD, pr.r(),
                            ph, itHJob, pr, itHJob);
                    }

                    // Executing transposition:
                    pr.insert(itHJob.key(), itHJob.value());
                    ph.erase(itHJob);

                    // Correcting characteristics
                    DurationType rr = pr.r();
                    DurationType lrs = *(pr.itS() - 1); // always available

                    // h-facility
                    ph.pImpl_.d -= lh;
                    ph.pImpl_.r += lh;
                    ph.pImpl_.t -= lh;
                    if (!ph.pImpl_.d) ph.pImpl_.r = 0;

                    // r-facility
                    --(pr.pImpl_.itS_);
                    pr.pImpl_.d = lh - rr;
                    pr.pImpl_.r += lrs - lh;
                    pr.pImpl_.t += lh;
                    if (!pr.pImpl_.d) pr.pImpl_.r = 0;

                    // schedule
                    F -= rr;
                    nRTot_ -= rr;
                    nDTot_ -= rr;

                    dFacs_.push_back(*itRFac);
                    itRFac = rFacs_.erase(itRFac);

                    break;
                }
            }
        }

        if (!bFound) ++itRFac;
        return bFound;
    }

    bool TAGSchedule.ScheduleImpl.transp_p1pRD(
        QList<TAGFacility*>.iterator & itRFac)
{
        bool bFound = false;

        TAGFacility* pr = *itRFac;

        // Search r-jobs in r-facility to swap with...
        TAGFacility.iterator itRJob = pr.begin();
        for (; itRJob != pr.itS(); ++itRJob)
        {

            // ... one among all h-jobs
            QList<TAGFacility*>.iterator itHFac = dFacs_.begin();
            for (; itHFac != dFacs_.end(); ++itHFac)
            {
                TAGFacility* ph = *itHFac;

                TAGFacility.iterator itHJob = ph.begin();
                for (; itHJob != ph.itS(); ++itHJob)
                {
                    DurationType deltaLen = itHJob.value() - itRJob.value();

                    if (deltaLen > pr.r() && deltaLen < ph.d())
                    {
                        bFound = true;
                        if (pObserver_)
                        {
                            postTransp(TranspObserver.p1 + TranspObserver.pRD, pr.r(),
                                ph, itHJob, pr, itRJob);
                        }

                        // Executing transposition:
                        ph.insert(itRJob.key(), itRJob.value());
                        pr.insert(itHJob.key(), itHJob.value());
                        ph.erase(itHJob);
                        pr.erase(itRJob);

                        // Correcting characteristics
                        DurationType rr = pr.r();
                        DurationType lrs = *(pr.itS() - 1); // always available

                        // h-facility
                        ph.pImpl_.d -= deltaLen;
                        ph.pImpl_.r += deltaLen;
                        ph.pImpl_.t -= deltaLen;
                        if (!ph.pImpl_.d) ph.pImpl_.r = 0;

                        // r-facility
                        --(pr.pImpl_.itS_);
                        pr.pImpl_.d = deltaLen - rr;
                        pr.pImpl_.r += lrs - deltaLen;
                        pr.pImpl_.t += deltaLen;
                        if (!pr.pImpl_.d) pr.pImpl_.r = 0;

                        // schedule
                        F -= rr;
                        nRTot_ -= rr;
                        nDTot_ -= rr;

                        dFacs_.push_back(*itRFac);
                        itRFac = rFacs_.erase(itRFac);

                        break;
                    }
                }

                if (bFound) break;
            }

            if (bFound) break;
        }

        if (!bFound) ++itRFac;
        return bFound;
    }

    bool TAGSchedule.ScheduleImpl.transp_p0pR_r(
        QList<TAGFacility*>.iterator & itRFac)
{
        bool bFound = false;

        // Search all h-jobs to move one to r-facility
        QList<TAGFacility*>.iterator itHFac = dFacs_.begin();
        for (; itHFac != dFacs_.end(); ++itHFac)
        {
            TAGFacility* ph = *itHFac;
            TAGFacility* pr = *itRFac;

            DurationType minRD = std.min(pr.r(), ph.d());
            JobIdx idxHJob =
                std.upper_bound(pl_.begin(), pl_.end(), minRD) - pl_.begin() - 1;
            if (idxHJob)
            {
                TAGFacility.iterator itHJob = ph.upperBound(idxHJob);
                if (itHJob != ph.end() && itHJob != ph.begin() &&
                     itHJob.key() <= ph.itS().key())
                {
                    bFound = true;
                    exec_p0pR(itHFac, --itHJob, itRFac);
                }
            }

            if (bFound) break;
        }

        ++itRFac;
        return bFound;
    }

    bool TAGSchedule.ScheduleImpl.transp_p0pR_h(
        QList<TAGFacility*>.iterator & itHFac)
{
        bool bFound = false;

        // Among all r-racilities find one where...
        QList<TAGFacility*>.iterator itRFac = rFacs_.begin();
        for (; itRFac != rFacs_.end(); ++itRFac)
        {
            TAGFacility* ph = *itHFac;
            TAGFacility* pr = *itRFac;

            // ...h-job from h-facility can be moved
            DurationType minRD = std.min(pr.r(), ph.d());
            JobIdx idxHJob =
                std.upper_bound(pl_.begin(), pl_.end(), minRD) - pl_.begin() - 1;
            if (idxHJob)
            {
                TAGFacility.iterator itHJob = ph.upperBound(idxHJob);
                if (itHJob != ph.end() && itHJob != ph.begin() &&
                     itHJob.key() <= ph.itS().key())
                {
                    bFound = true;
                    exec_p0pR(itHFac, --itHJob, itRFac);
                }
            }

            if (bFound) break;

        }

        if (!bFound) ++itHFac; // |Id|' = |Id| - 1 if found
        return bFound;
    }

    void TAGSchedule.ScheduleImpl.exec_p0pR(QList<TAGFacility*>.iterator & itHFac, TAGFacility.iterator itHJob,
                                            QList<TAGFacility*>.iterator & itRFac)
    {
        TAGFacility* pr = *itRFac;
        TAGFacility* ph = *itHFac;
        DurationType lh = itHJob.value();
        if (pObserver_)
        {
            postTransp(TranspObserver.p0 + TranspObserver.pR, lh,
                ph, itHJob, pr, itHJob);
        }

        // Executing transposition
        pr.insert(itHJob.key(), itHJob.value());
        ph.erase(itHJob);

        // Correcting characteristics
        // h-facility
        ph.pImpl_.d -= lh;
        ph.pImpl_.r += lh;
        ph.pImpl_.t -= lh;

        // r-facility
        pr.pImpl_.d += lh;
        pr.pImpl_.r -= lh;
        pr.pImpl_.t += lh;
        if (!pr.pImpl_.r) pr.pImpl_.d = 0;

        // schedule
        F -= lh;
        nRTot_ -= lh;
        nDTot_ -= lh;

        if (!ph.pImpl_.d)
        {
            ph.pImpl_.r = 0;

            FacIdx idxRFac = itRFac - rFacs_.begin();
            rFacs_.push_front(*itHFac);
            itRFac = rFacs_.begin() + idxRFac;
            itHFac = dFacs_.erase(itHFac);
        }
    }

    bool TAGSchedule.ScheduleImpl.transp_p1pR(QList<TAGFacility*>.iterator & itRFac)
    {
        bool bFound = false;

        TAGFacility* pr = *itRFac;

        // Search r-jobs in r-facility to swap them with...
        TAGFacility.iterator itRJob = pr.begin();
        for (; itRJob != pr.itS(); ++itRJob)
        {

            // ... one among all h-jobs in h-facilities
            QList<TAGFacility*>.iterator itHFac = dFacs_.begin();
            for (; itHFac != dFacs_.end(); ++itHFac)
            {
                TAGFacility* ph = *itHFac;

                TAGFacility.iterator itHJob = ph.begin();
                for (; itHJob != ph.itS(); ++itHJob)
                {
                    DurationType deltaLen = itHJob.value() - itRJob.value();

                    if (deltaLen <= pr.r() && deltaLen <= ph.d() && deltaLen > 0)
                    {
                        bFound = true;
                        if (pObserver_)
                        {
                            postTransp(TranspObserver.p1 + TranspObserver.pR, deltaLen,
                                ph, itHJob, pr, itHJob);
                        }

                        // Executing transposition
                        ph.insert(itRJob.key(), itRJob.value());
                        pr.insert(itHJob.key(), itHJob.value());
                        ph.erase(itHJob);
                        pr.erase(itRJob);

                        // Correcting characteristics
                        // h-facility
                        ph.pImpl_.d -= deltaLen;
                        ph.pImpl_.r += deltaLen;
                        ph.pImpl_.t -= deltaLen;

                        // r-facility
                        pr.pImpl_.d += deltaLen;
                        pr.pImpl_.r -= deltaLen;
                        pr.pImpl_.t += deltaLen;
                        if (!pr.pImpl_.r) pr.pImpl_.d = 0;

                        // schedule
                        F -= deltaLen;
                        nRTot_ -= deltaLen;
                        nDTot_ -= deltaLen;

                        if (!ph.pImpl_.d)
                        {
                            ph.pImpl_.r = 0;

                            FacIdx idxRFac = itRFac - rFacs_.begin();
                            rFacs_.push_front(*itHFac);
                            itRFac = rFacs_.begin() + idxRFac;
                            itHFac = dFacs_.erase(itHFac);
                        }

                        break;
                    }
                }

                if (bFound) break;
            }

            if (bFound) break;
        }

        ++itRFac;
        return bFound;
    }

    bool TAGSchedule.ScheduleImpl.transp_s1pW1(QList<TAGFacility*>.iterator & itHFac)
    {
        bool bFound = false;

        TAGFacility* ph = *itHFac;
        DurationType ls = ph.itS().value();

        QList<TAGFacility*>.iterator itRFac = rFacs_.begin();
        for (; itRFac != rFacs_.end(); ++itRFac)
        {
            TAGFacility* pr = *itRFac;

            TAGFacility.iterator itRJob = pr.begin();
            for (; itRJob != pr.itS(); ++itRJob)
            {
                DurationType lr = itRJob.value();
                DurationType deltaLen = ls - lr;

                if (deltaLen <= pr.r() && deltaLen > 0 && lr <= ph.r())
                {
                    bFound = true;
                    if (pObserver_)
                    {
                        postTransp(TranspObserver.s1W1, ls - ph.r(),
                                    ph, ph.itS(), pr, itRJob);
                    }

                    // Executing transposition
                    TAGFacility.iterator itHS = ph.pImpl_.itS_;

                    pr.insert(itHS.key(), itHS.value());
                    ph.insert(itRJob.key(), itRJob.value());
                    pr.erase(itRJob);

                    ++(ph.pImpl_.itS_);     // always available
                    ph.erase(itHS);

                    // Correcting characteristics
                    DurationType lhs = ph.itS().value();
                    DurationType rh = ph.r();
                    DurationType dh = ph.d();

                    // h-facility
                    ph.pImpl_.d += lhs - deltaLen;
                    ph.pImpl_.r = deltaLen - dh;
                    ph.pImpl_.t -= deltaLen;
                    if (!ph.pImpl_.r) ph.pImpl_.d = 0;

                    // r-facility
                    pr.pImpl_.d += deltaLen;
                    pr.pImpl_.r -= deltaLen;
                    pr.pImpl_.t += deltaLen;
                    if (!pr.pImpl_.r) pr.pImpl_.d = 0;

                    // schedule
                    F -= ls - rh;
                    nRTot_ -= dh;
                    nDTot_ -= dh;

                    rFacs_.push_front(*itHFac);
                    itHFac = dFacs_.erase(itHFac);

                    break;
                }
            }

            if (bFound) break;
        }

        if (!bFound) ++itHFac;
        return bFound;
    }

    bool TAGSchedule.ScheduleImpl.transp_s1pW2(QList<TAGFacility*>.iterator & itHFac)
    {
        bool bFound = false;

        TAGFacility* ph = *itHFac;
        DurationType ls = ph.itS().value();

        QList<TAGFacility*>.iterator itRFac = rFacs_.begin();
        for (; itRFac != rFacs_.end(); ++itRFac)
        {
            TAGFacility* pr = *itRFac;

            TAGFacility.iterator itRJob = pr.begin();
            for (; itRJob != pr.itS(); ++itRJob)
            {
                DurationType lr = itRJob.value();
                DurationType deltaLen = ls - lr;

                if (deltaLen <= pr.r() && deltaLen > 0 && lr > ph.r())
                {
                    bFound = true;
                    if (pObserver_)
                    {
                        postTransp(TranspObserver.s1W2, deltaLen,
                                    ph, ph.itS(), pr, itRJob);
                    }

                    // Executing transposition
                    TAGFacility.iterator itHS = ph.pImpl_.itS_;

                    pr.insert(itHS.key(), itHS.value());
                    ph.insert(itRJob.key(), itRJob.value());
                    pr.erase(itRJob);

                    --(ph.pImpl_.itS_);     // always available
                    ph.erase(itHS);

                    // Correcting characteristics
                    DurationType lhs = ph.itS().value();
                    DurationType rh = ph.r();

                    // h-facility
                    ph.pImpl_.d = lr - rh;
                    ph.pImpl_.r += lhs - lr;
                    ph.pImpl_.t -= deltaLen;
                    if (!ph.pImpl_.d) ph.pImpl_.r = 0;

                    // r-facility
                    pr.pImpl_.d += deltaLen;
                    pr.pImpl_.r -= deltaLen;
                    pr.pImpl_.t += deltaLen;
                    if (!pr.pImpl_.r) pr.pImpl_.d = 0;

                    // schedule
                    F -= deltaLen;
                    nRTot_ -= deltaLen;
                    nDTot_ -= deltaLen;

                    break;
                }
            }

            if (bFound) break;
        }

        ++itHFac; // for compatibility with other transpositions
        return bFound;

    bool TAGSchedule.ScheduleImpl.already_optimized()
    {
            if (!Q())
            {
                bOptimized = true;
            }

            return bOptimized;
        }
    }
}

internal class Program
{
        static void Main(string[] args)
        {
            Console.WriteLine("МСЗП Кости Шевченко на С++ ;)"); 
        }
}
