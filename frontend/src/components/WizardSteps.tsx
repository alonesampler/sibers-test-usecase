export function WizardSteps({
  steps,
  current,
}: {
  steps: readonly string[]
  current: number
}) {
  return (
    <ol className="wizard-steps" aria-label="Шаги мастера">
      {steps.map((label, index) => {
        const step = index + 1
        const done = step < current
        const active = step === current
        return (
          <li
            key={label}
            className={['wizard-step', done && 'wizard-step-done', active && 'wizard-step-active']
              .filter(Boolean)
              .join(' ')}
            aria-current={active ? 'step' : undefined}
          >
            <span className="wizard-step-num">{done ? '✓' : step}</span>
            <span className="wizard-step-label">{label}</span>
          </li>
        )
      })}
    </ol>
  )
}