import Fab from "@mui/material/Fab";
import AddIcon from "@mui/icons-material/Add";
import { useState } from "react";
import Modal from "@mui/material/Modal";
import MakeRecommendationModal from "./make-recommendation-modal";

const MakeARecommendation = () => {
  const [modalOpen, setModalOpen] = useState(false);
  const toggleModal = (current: boolean) => setModalOpen(!current);

  return (
    <>
      <Fab
        color="primary"
        aria-label="add"
        variant="extended"
        onClick={() => {
          toggleModal(modalOpen);
        }}
      >
        <AddIcon />
        Make A Recommendation
      </Fab>

      <Modal
        open={modalOpen}
        onClose={() => {
          toggleModal(modalOpen);
        }}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <MakeRecommendationModal
          closeModal={() => {
            toggleModal(modalOpen);
          }}
        />
      </Modal>
    </>
  );
};

export default MakeARecommendation;
